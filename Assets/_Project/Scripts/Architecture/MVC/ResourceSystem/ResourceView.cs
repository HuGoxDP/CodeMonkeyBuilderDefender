using System.Collections.Generic;
using _Project.Scripts.Architecture.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Architecture.MVC.ResourceSystem
{
    public class ResourceView : MonoBehaviour
    {
        private const float XOffsetAmount = -170;
        [SerializeField] private Transform _resourceUIPrefab;
        private IResourceModel _resourceModel;
        private ResourceTypeListSo _resourceTypeList;

        private Dictionary<ResourceTypeSo, ResourceUITemplate> _resourceUIDictionary;

        private void OnDestroy()
        {
            _resourceModel.OnResourceAmountChanged -= UpdateUI;
        }

        private void CreateUI()
        {
            for (var index = 0; index < _resourceTypeList.List.Count; index++)
            {
                var resourceTypeSo = _resourceTypeList.List[index];

                var resourceTransform = Instantiate(_resourceUIPrefab, transform);
                var resourceUITemplate = resourceTransform.GetComponent<ResourceUITemplate>();
                resourceTransform.gameObject.SetActive(true);
                resourceTransform.GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(-50 + XOffsetAmount * index, 0);
                resourceUITemplate.Image.sprite = resourceTypeSo.ResourceSprite;
                resourceUITemplate.ResourceAmount.SetText("0");
                _resourceUIDictionary[resourceTypeSo] = resourceUITemplate;
            }
        }

        private void UpdateUI((ResourceTypeSo resourceType, int currentAmount) valueTuple)
        {
            var (resourceType, currentAmount) = valueTuple;
            _resourceUIDictionary[resourceType].ResourceAmount.SetText(currentAmount.ToString());
        }

        public void Initialize(IResourceModel resourceModel, ResourceTypeListSo resourceTypeList)
        {
            _resourceUIDictionary = new Dictionary<ResourceTypeSo, ResourceUITemplate>();
            _resourceTypeList = resourceTypeList;
            _resourceModel = resourceModel;

            CreateUI();
            _resourceModel.OnResourceAmountChanged += UpdateUI;
        }
    }
}