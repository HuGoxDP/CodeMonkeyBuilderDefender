using System;
using System.Threading;
using _Project.Scripts.Architecture.InputReader;
using _Project.Scripts.Architecture.MVC.ResourceSystem;
using _Project.Scripts.Architecture.ScriptableObjects;
using _Project.Scripts.Architecture.UnityServiceLocator;
using _Project.Scripts.Architecture.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Architecture.MVC.BuildingSystem
{
    public class BuildingManager : MonoBehaviour
    {
        [SerializeField] private BuildingView _buildingView;
        private IBuildingInputReader _buildingInputReader;

        private IBuildingModel _buildingModel;

        private BuildingTypeSo _buildingType;
        private BuildingTypeListSo _buildingTypeList;
        private Camera _camera;
        private CancellationTokenSource _cts;
        private bool _isBuilding;

        private void Awake()
        {
            _cts = new CancellationTokenSource();
            _buildingModel = new BuildingModel();
            InitializeBuildingSystem().Forget();
        }

        private void Start()
        {
            var serviceLocator = ServiceLocator.Instance;

            if (serviceLocator == null)
            {
                Debug.LogError("BuildingManager.Start: Service Locator is null");
                return;
            }

            serviceLocator.GetService(out IInputManager inputManager);
            serviceLocator.GetService(out ResourceManager resourceManager);
            serviceLocator.GetService(out BuildingGhost buildingGhost);

            buildingGhost.Initialize(this);
            InitializeInput(inputManager);
            InitializeBuildingModel(resourceManager);
            InitializeCamera();

            if (_buildingView != null)
            {
                _buildingView.BuildingTypeSelected += SelectBuildingType;
            }
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();

            if (_buildingModel == null)
            {
                Debug.LogError("BuildingManager.OnDestroy: BuildingModel is null");
            }
            else
            {
                _buildingModel.OnBuildingError -= BuildingModelOnOnBuildingError;
                _buildingModel.OnBuildingAdded -= BuildingModelOnOnBuildingAdded;
                _buildingModel.OnBuildingRemoved -= BuildingModelOnOnBuildingRemoved;
                _buildingModel.OnError -= BuildingModelOnOnError;
            }

            if (_buildingView == null)
            {
                Debug.LogError("BuildingManager.OnDestroy: BuildingView is null");
            }
            else
            {
                _buildingView.BuildingTypeSelected -= SelectBuildingType;
            }

            if (_buildingInputReader == null)
            {
                Debug.LogError("BuildingManager.OnDestroy: BuildingInputReader is null");
            }
            else
            {
                _buildingInputReader.Place -= BuildBuilding;
                _buildingInputReader.Remove -= DemolishBuilding;
            }
        }

        public event Action<BuildingTypeSo> BuildingTypeSelected;

        private void InitializeCamera()
        {
            _camera = Camera.main ?? FindFirstObjectByType<Camera>();

            if (_camera != null) return;

            Debug.LogError("BuildingManager.InitializeCamera: No camera found in scene!");
            enabled = false;
        }

        private void InitializeBuildingModel(ResourceManager resourceManager)
        {
            if (resourceManager == null)
            {
                Debug.LogError("BuildingManager.InitializeBuildingModel: resourceManager is null");
                return;
            }

            if (_buildingModel == null)
            {
                Debug.LogError("BuildingManager.InitializeBuildingModel: BuildingModel is null");
                return;
            }

            _buildingModel.SetResourceManager(resourceManager);
            _buildingModel.OnBuildingError += BuildingModelOnOnBuildingError;
            _buildingModel.OnBuildingAdded += BuildingModelOnOnBuildingAdded;
            _buildingModel.OnBuildingRemoved += BuildingModelOnOnBuildingRemoved;
            _buildingModel.OnError += BuildingModelOnOnError;
        }

        private void InitializeInput(IInputManager inputManager)
        {
            if (inputManager == null)
            {
                Debug.LogError("BuildingManager.InitializeInput: inputManager is null");
                return;
            }

            _buildingInputReader = inputManager.BuildingInputReader;

            if (_buildingInputReader == null)
            {
                Debug.LogError("BuildingManager.InitializeInput: BuildingInputReader is null");
                return;
            }

            _buildingInputReader.Place += BuildBuilding;
            _buildingInputReader.Remove += DemolishBuilding;
        }

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

        private async UniTaskVoid InitializeBuildingSystem()
        {
            try
            {
                _buildingTypeList = await AssetManager.Instance
                    .LoadAsset<BuildingTypeListSo>(nameof(BuildingTypeListSo));

                if (_buildingTypeList == null)
                {
                    throw new Exception("Building type list is null!");
                }

                if (_buildingView != null)
                {
                    _buildingView.Initialize(_buildingTypeList);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Building system init failed: {e.Message}");
            }
        }

        private void SelectBuildingType(BuildingTypeSo buildingType)
        {
            _buildingType = buildingType;
            BuildingTypeSelected?.Invoke(_buildingType);
        }

        //Build a building at the mouse position
        private void BuildBuilding()
        {
            if (_buildingType == null || _isBuilding)
                return;

            BuildAsync(
                _buildingType,
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

            // - Remove building GameObject.
        }
    }
}