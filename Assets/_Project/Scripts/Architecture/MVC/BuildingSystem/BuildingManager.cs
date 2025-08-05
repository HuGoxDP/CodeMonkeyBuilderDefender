using System;
using System.Threading;
using _Project.Scripts.Architecture.DI;
using _Project.Scripts.Architecture.InputReader;
using _Project.Scripts.Architecture.Interfaces;
using _Project.Scripts.Architecture.ScriptableObjects;
using _Project.Scripts.Architecture.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Architecture.MVC.BuildingSystem
{
    public class BuildingManager : MonoBehaviour
    {
        [SerializeField] private BuildingGhost _buildingGhost;
        [SerializeField] private BuildingView _buildingView;
        private IBuildingInputReader _buildingInputReader;

        private IBuildingModel _buildingModel;
        private IBuildingTypeProvider _buildingTypeProvider;
        private Camera _camera;
        private CancellationTokenSource _cts;

        private BuildingTypeSo _currentBuildingType;
        private bool _isBuilding;

        private void Start()
        {
            InitializeWithDI();
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();

            if (_buildingModel != null)
            {
                _buildingModel.OnBuildingError -= BuildingModelOnOnBuildingError;
                _buildingModel.OnBuildingAdded -= BuildingModelOnOnBuildingAdded;
                _buildingModel.OnBuildingRemoved -= BuildingModelOnOnBuildingRemoved;
                _buildingModel.OnError -= BuildingModelOnOnError;
            }

            if (_buildingView != null)
            {
                _buildingView.BuildingTypeSelected -= SelectBuildingType;
            }

            if (_buildingInputReader != null)
            {
                _buildingInputReader.Place -= BuildBuilding;
                _buildingInputReader.Remove -= DemolishBuilding;
            }
        }

        public event Action<BuildingTypeSo> BuildingTypeSelected;

        private void BuildingModelOnOnError(string obj)
        {
            Debug.LogError(obj);
        }

        private void BuildingModelOnOnBuildingRemoved(Building obj)
        {
            //TODO добавить звук при продаже постройки
            Debug.Log($"BuildingModel.OnBuildingRemoved: {obj.name}");
        }

        private void BuildingModelOnOnBuildingAdded(Building obj)
        {
            //TODO добавить звук при постройке
            Debug.Log($"BuildingModel.OnBuildingAdded: {obj.name}");
        }

        private void BuildingModelOnOnBuildingError()
        {
            //TODO добавить звук ошибки при постройке
            Debug.Log($"BuildingModel.OnBuildingError");
        }

        private void SelectBuildingType(BuildingTypeSo buildingType)
        {
            _currentBuildingType = buildingType;
            BuildingTypeSelected?.Invoke(_currentBuildingType);
        }

        //Build a building at the mouse position
        private void BuildBuilding()
        {
            if (_currentBuildingType == null || _isBuilding)
                return;

            BuildAsync(
                _currentBuildingType,
                WorldPositionUtils.ScreenToWorldPosition(_camera, _buildingInputReader.PointPosition)
            ).Forget();
        }

        private async UniTaskVoid BuildAsync(BuildingTypeSo type, Vector3 pos)
        {
            _isBuilding = true;

            try
            {
                if (await _buildingModel.CanBuild(type, pos))
                {
                    var newBuilding = await BuildingFactory.CreateBuildingAsync(
                        type,
                        pos,
                        Quaternion.identity,
                        transform,
                        _cts.Token
                    );
                    _buildingModel.AddBuilding(newBuilding);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Building cancelled");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Building failed: {ex.Message}");
            }
            finally
            {
                _isBuilding = false;
            }
        }

        // Demolish an existing building and recovering resources
        private void DemolishBuilding()
        {
            var buildingPos = WorldPositionUtils.ScreenToWorldPosition(_camera, _buildingInputReader.PointPosition);
            var building = _buildingModel.GetBuildingAt(buildingPos);
            if (building != null)
            {
                _buildingModel.RemoveBuilding(building);
                Destroy(building.gameObject);
            }
        }

        #region Initialization

        // ReSharper disable once InconsistentNaming
        private void InitializeWithDI()
        {
            try
            {
                var container = DIContainer.Instance;

                var inputManager = container.Resolve<IInputManager>();
                var gameResourceManager = container.Resolve<IGameResourceManager>();
                _buildingTypeProvider = container.Resolve<IBuildingTypeProvider>();

                InitializeBuildingManager(inputManager, gameResourceManager);
            }
            catch (Exception ex)
            {
                Debug.LogError($"DIResourceHarvester: Failed to resolve dependencies - {ex.Message}");
            }
        }

        private void InitializeBuildingManager(IInputManager inputManager, IGameResourceManager gameResourceManager)
        {
            _cts = new CancellationTokenSource();
            _buildingModel = new BuildingModel();

            _buildingView.Initialize(_buildingTypeProvider);
            _buildingInputReader = inputManager.BuildingInputReader;


            _buildingGhost.Initialize(this, _buildingInputReader);

            InitializeInput();
            InitializeBuildingModel(gameResourceManager);
            InitializeCamera();

            if (_buildingView != null)
            {
                _buildingView.BuildingTypeSelected += SelectBuildingType;
            }
        }

        private void InitializeCamera()
        {
            _camera = Camera.main ?? FindFirstObjectByType<Camera>();

            if (_camera != null) return;

            Debug.LogError("BuildingManager.InitializeCamera: No camera found in scene!");
            enabled = false;
        }

        private void InitializeBuildingModel(IGameResourceManager gameResourceManager)
        {
            _buildingModel.SetResourceManager(gameResourceManager);
            _buildingModel.OnBuildingError += BuildingModelOnOnBuildingError;
            _buildingModel.OnBuildingAdded += BuildingModelOnOnBuildingAdded;
            _buildingModel.OnBuildingRemoved += BuildingModelOnOnBuildingRemoved;
            _buildingModel.OnError += BuildingModelOnOnError;
        }

        private void InitializeInput()
        {
            _buildingInputReader.Place += BuildBuilding;
            _buildingInputReader.Remove += DemolishBuilding;
        }

        #endregion
    }
}