using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Architecture.MVC.BuildingManager
{
    public class BuildingView : MonoBehaviour
    {
        public event Action<BuildingTypeSO> BuildingTypeSelected;
        
        [SerializeField] private Transform _buildingUIPrefab;

        private const float XOffsetAmount = 110;
        
        private Dictionary<BuildingTypeSO, BuildingUITemplate> _buildingUIDictionary;
        private BuildingTypeListSO _buildingTypeList;


        private void Awake()
        {
            _buildingTypeList = Resources.Load<BuildingTypeListSO>(nameof(BuildingTypeListSO));
            _buildingUIDictionary = new Dictionary<BuildingTypeSO, BuildingUITemplate>();

            CreateUI();
        }

        private void CreateUI()
        {
            for (var index = 0; index < _buildingTypeList.List.Count; index++)
            {
                var buildingType = _buildingTypeList.List[index];

                var buildingUITransform = Instantiate(_buildingUIPrefab, transform);
                var buildingUITemplate = buildingUITransform.GetComponent<BuildingUITemplate>();
                buildingUITransform.gameObject.SetActive(true);
                buildingUITransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(XOffsetAmount * index, 0);
                buildingUITemplate.BuildingImage.sprite = buildingType.Icon;
                buildingUITemplate.Button.onClick.AddListener(() => OnBuildingTypeSelected(buildingType));
                _buildingUIDictionary[buildingType] = buildingUITemplate;
            }
        }

        private void OnBuildingTypeSelected(BuildingTypeSO buildingType)
        {
            BuildingTypeSelected?.Invoke(buildingType);
        }
        
    }
}