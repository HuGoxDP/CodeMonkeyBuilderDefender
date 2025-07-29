using System;
using _Project.Scripts.Architecture.InputReader;
using _Project.Scripts.Architecture.ScriptableObjects;
using _Project.Scripts.Architecture.UnityServiceLocator;
using _Project.Scripts.Architecture.Utils;
using UnityEngine;

namespace _Project.Scripts.Architecture.MVC.BuildingSystem
{
    public class BuildingGhost : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sprite;
        private BuildingManager _buildingManager;
        private IPointerPositionInputReader _pointerPositionInputReader;

        private void Awake()
        {
            Hide();
        }

        private void Start()
        {
            ServiceLocator.Instance.GetService(out IInputManager inputManager);
            _pointerPositionInputReader = inputManager.BuildingInputReader;
            if (_pointerPositionInputReader == null)
            {
                throw new NullReferenceException("_pointerPositionInputReader");
            }
        }

        private void Update()
        {
            transform.position = WorldPositionUtils.ScreenToWorldPosition(_pointerPositionInputReader.PointPosition);
        }

        private void OnDestroy()
        {
            if (_buildingManager != null)
            {
                _buildingManager.BuildingTypeSelected -= ManagerOnBuildingTypeSelected;
            }
        }

        public void Initialize(BuildingManager manager)
        {
            _buildingManager = manager ?? throw new ArgumentNullException(nameof(manager));
            manager.BuildingTypeSelected += ManagerOnBuildingTypeSelected;
        }

        private void ManagerOnBuildingTypeSelected(BuildingTypeSo building)
        {
            if (building != null)
            {
                Show(building.Icon);
            }
            else
            {
                Hide();
            }
        }

        private void Show(Sprite ghostSprite)
        {
            _sprite.gameObject.SetActive(true);
            _sprite.sprite = ghostSprite;
        }

        private void Hide()
        {
            _sprite.gameObject.SetActive(false);
        }
    }
}