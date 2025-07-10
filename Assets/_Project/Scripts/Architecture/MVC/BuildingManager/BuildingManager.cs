using System;
using System.Threading;
using _Project.Scripts.Architecture.InputReader;
using _Project.Scripts.Architecture.ScriptableObjects;
using _Project.Scripts.Architecture.UnityServiceLocator;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Architecture.MVC.BuildingManager
{
    public class BuildingManager : MonoBehaviour
    {
        [SerializeField] private BuildingView _buildingView;

        private IBuildingInputReader _buildingInputReader;
        private BuildingTypeSo _buildingType;
        private BuildingTypeListSo _buildingTypeList;
        private Camera _camera;
        private CancellationTokenSource _cts;

        private bool _isBuilding;

        private void Awake()
        {
            _cts = new CancellationTokenSource();
            InitializeBuildingSystem().Forget();
        }

        private void Start()
        {
            ServiceLocator.Instance.GetService(out IInputManager inputManager);
            _buildingInputReader = inputManager.BuildingInputReader;

            _camera = Camera.main;

            if (_buildingView != null)
            {
                _buildingView.BuildingTypeSelected += SelectBuildingType;
            }

            if (_buildingInputReader != null)
            {
                _buildingInputReader.Place += BuildBuilding;
                _buildingInputReader.Remove += DemolishBuilding;
            }
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();

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

        private async UniTaskVoid InitializeBuildingSystem()
        {
            try
            {
                _buildingTypeList = await AssetManager.Instance
                    .LoadAsset<BuildingTypeListSo>(nameof(BuildingTypeListSo));

                _buildingType = _buildingTypeList.List[0];

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
        }

        //Build a building at the mouse position
        private void BuildBuilding()
        {
            if (_buildingType == null || _isBuilding) return;

            BuildAsync().Forget();
        }

        private async UniTaskVoid BuildAsync()
        {
            _isBuilding = true;
            try
            {
                if (!CanBuild(_buildingType))
                {
                    Debug.Log("Cannot build here!");
                    return;
                }

                Transform newBuilding = await _buildingType.InstantiateAsync(
                    transform,
                    GetMouseWorldPosition(),
                    Quaternion.identity
                ).AttachExternalCancellation(_cts.Token);

                if (newBuilding != null)
                {
                    ApplyBuildCost(_buildingType);
                    Debug.Log($"Building created: {newBuilding.name}");
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Building cancelled");
            }
            finally
            {
                _isBuilding = false;
            }
        }

        private void ApplyBuildCost(BuildingTypeSo buildingType)
        {
            // TODO: Вычесть ресурсы
        }

        private bool CanBuild(BuildingTypeSo buildingType)
        {
            // TODO: Реализовать проверку
            // - Достаточно ли ресурсов
            // - Нет ли коллизий
            // - Валидна ли позиция
            return true;
        }

        // Demolish an existing building and recovering resources
        private void DemolishBuilding()
        {
            // TODO: Check if there are buildings (USING PHYSICS2D).
            // - Find building in BuildingTypeListSO.
            // - Get resources amounting.
            // - Add resources to stash.
            // - Remove building GameObject.
        }

        // Get mouse position in world space
        private Vector3 GetMouseWorldPosition()
        {
            var mouseWorldPosition = _camera.ScreenToWorldPoint(_buildingInputReader.PointPosition);
            mouseWorldPosition.z = 0f;
            return mouseWorldPosition;
        }
    }
}