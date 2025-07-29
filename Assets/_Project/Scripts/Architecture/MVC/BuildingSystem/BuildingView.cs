using System;
using System.Collections.Generic;
using _Project.Scripts.Architecture.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Architecture.MVC.BuildingSystem
{
    public class BuildingView : MonoBehaviour
    {
        private const float XOffsetAmount = 110;

        [SerializeField] private Transform _buildingUIPrefab;
        [SerializeField] private Sprite _arrowSprite;

        private BuildingTypeListSo _buildingTypeList;
        private Dictionary<string, BuildingUITemplate> _buildingUIDictionary;
        private BuildingUITemplate _selected;
        private int _uiIndex = 0;

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
            foreach (var t in _buildingTypeList.List)
            {
                var buildingType = t;
                CreateCustomUI(_uiIndex, t.Icon, buildingType, buildingType.NameString);
            }
        }

        private void CreateCustomUI(int index, Sprite icon, BuildingTypeSo type, string uiName)
        {
            var uiTransform = Instantiate(_buildingUIPrefab, transform);
            if (uiTransform.TryGetComponent<RectTransform>(out var rectTransform))
            {
                rectTransform.anchoredPosition = new Vector2(XOffsetAmount * index, 0);
            }

            if (uiTransform.TryGetComponent<BuildingUITemplate>(out var buildingUITemplate))
            {
                buildingUITemplate.BuildingImage.sprite = icon;
                buildingUITemplate.Button.onClick.AddListener(() => OnBuildingTypeSelected(type, uiName));
            }

            uiTransform.gameObject.SetActive(true);
            _buildingUIDictionary[uiName] = buildingUITemplate;
            _uiIndex++;
        }

        private void OnBuildingTypeSelected(BuildingTypeSo buildingType, string uiName)
        {
            UpdateActiveBuildingTypeButton(uiName);
            BuildingTypeSelected?.Invoke(buildingType);
        }

        private void UpdateActiveBuildingTypeButton(string buildingType)
        {
            if (_selected != null)
            {
                _selected.Deselect();
            }

            _selected = _buildingUIDictionary[buildingType];
            _selected.Select();
        }

        public void Initialize(BuildingTypeListSo buildingTypeList)
        {
            _buildingUIDictionary = new Dictionary<string, BuildingUITemplate>();
            _buildingTypeList = buildingTypeList;

            CreateCustomUI(_uiIndex, _arrowSprite, null, "Arrow");
            CreateUI();

            if (_buildingTypeList.List.Count > 0)
            {
                OnBuildingTypeSelected(null, "Arrow");
            }
        }
    }
}