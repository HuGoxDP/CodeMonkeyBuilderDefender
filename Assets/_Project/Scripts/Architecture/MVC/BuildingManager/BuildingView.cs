using System;
using System.Collections.Generic;
using _Project.Scripts.Architecture.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Architecture.MVC.BuildingManager
{
    public class BuildingView : MonoBehaviour
    {
        private const float XOffsetAmount = 110;

        [SerializeField] private Transform _buildingUIPrefab;
        private BuildingTypeListSo _buildingTypeList;

        private Dictionary<BuildingTypeSo, BuildingUITemplate> _buildingUIDictionary;

        private void OnDestroy()
        {
            foreach (var building in _buildingUIDictionary.Values)
            {
                building.Button.onClick.RemoveAllListeners();
            }
        }

        public event Action<BuildingTypeSo> BuildingTypeSelected;

        private void CreateUI()
        {
            for (var index = 0; index < _buildingTypeList.List.Count; index++)
            {
                var buildingType = _buildingTypeList.List[index];
                var buildingUITransform = Instantiate(_buildingUIPrefab, transform);

                if (buildingUITransform.TryGetComponent<RectTransform>(out var rectTransform))
                {
                    rectTransform.anchoredPosition = new Vector2(XOffsetAmount * index, 0);
                }

                if (buildingUITransform.TryGetComponent<BuildingUITemplate>(out var buildingUITemplate))
                {
                    buildingUITemplate.BuildingImage.sprite = buildingType.Icon;
                    buildingUITemplate.Button.onClick.AddListener(() => OnBuildingTypeSelected(buildingType));
                }

                buildingUITransform.gameObject.SetActive(true);
                _buildingUIDictionary[buildingType] = buildingUITemplate;
            }
        }

        private void OnBuildingTypeSelected(BuildingTypeSo buildingType)
        {
            BuildingTypeSelected?.Invoke(buildingType);
        }

        public void Initialize(BuildingTypeListSo buildingTypeList)
        {
            _buildingUIDictionary = new Dictionary<BuildingTypeSo, BuildingUITemplate>();
            _buildingTypeList = buildingTypeList;
            CreateUI();
        }
    }
}