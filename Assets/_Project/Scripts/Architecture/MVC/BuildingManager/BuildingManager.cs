using _Project.Scripts.Architecture.InputReader;
using _Project.Scripts.Architecture.UnityServiceLocator;
using UnityEngine;

namespace _Project.Scripts.Architecture.MVC.BuildingManager
{
    public class BuildingManager : MonoBehaviour
    {
        [SerializeField] private BuildingView _buildingView;
        
        private IBuildingInputReader _buildingInputReader;
        private Camera _camera;
        private BuildingTypeListSO _buildingTypeList;
        private BuildingTypeSO _buildingType;

        private void Awake()
        {
            _buildingTypeList = Resources.Load<BuildingTypeListSO>(nameof(BuildingTypeListSO));
            _buildingType = _buildingTypeList.List[0];
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

        private void SelectBuildingType(BuildingTypeSO buildingType)
        {
            _buildingType =  buildingType;
        }

        private void OnDestroy()
        {
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

        //Build a building at the mouse position
        private void BuildBuilding()
        {
            if (_buildingType != null)
            {
                _buildingType.Instantiate(transform, GetMouseWorldPosition(), Quaternion.identity);
            }
        }

        // Demolish an existing building and recovering resources
        private void DemolishBuilding()
        {
            //TODO Check if there are buildings (USING PHYSICS2D).
            //TODO Find building in BuildingTypeListSO.
            //TODO Get resources amounting.
            //TODO Add resources to stash.
            //TODO Remove building GameObject.
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