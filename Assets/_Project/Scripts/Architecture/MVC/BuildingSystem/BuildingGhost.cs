using System;
using _Project.Scripts.Architecture.InputReader;
using _Project.Scripts.Architecture.Interfaces;
using _Project.Scripts.Architecture.ScriptableObjects;
using _Project.Scripts.Architecture.Utils;
using UnityEngine;

namespace _Project.Scripts.Architecture.MVC.BuildingSystem
{
    public class BuildingGhost : MonoBehaviour, IBuildingGhost
    {
        [SerializeField] private SpriteRenderer _sprite;

        private BuildingManager _buildingManager;

        private bool _isInitialized = false;
        private IPointerPositionInputReader _pointerPositionInputReader;

        private void Start()
        {
            Hide();
        }

        private void Update()
        {
            if (_isInitialized)
            {
                transform.position =
                    WorldPositionUtils.ScreenToWorldPosition(_pointerPositionInputReader.PointPosition);
            }
        }

        private void OnDestroy()
        {
            if (_buildingManager)
            {
                _buildingManager.BuildingTypeSelected -= ManagerOnBuildingTypeSelected;
            }
        }

        public void Initialize(BuildingManager manager, IPointerPositionInputReader pointerPositionInputReader)
        {
            if (_isInitialized)
                return;

            _pointerPositionInputReader = pointerPositionInputReader ?? throw new ArgumentNullException(
                nameof(pointerPositionInputReader),
                $"BuildingGhost:Initialize: IPointerPositionInputReader is null"
            );
            _buildingManager = manager ?? throw new ArgumentNullException(
                nameof(manager),
                $"BuildingGhost:Initialize: BuildingManager is null"
            );

            manager.BuildingTypeSelected += ManagerOnBuildingTypeSelected;

            _isInitialized = true;
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