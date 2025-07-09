using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Architecture.MVC.ResourceManager
{
    public class ResourceView : MonoBehaviour
    {
        [SerializeField] private Transform _resourceUIPrefab;
        
        private const float XOffsetAmount = -170;
        
        private Dictionary<ResourceTypeSO, ResourceUITemplate> _resourceUIDictionary;
        private IResourceModel _resourceModel;
        private ResourceTypeListSO _resourceTypeList;
        
        private void Awake()
        {
            _resourceTypeList = Resources.Load<ResourceTypeListSO>(nameof(ResourceTypeListSO));
            _resourceUIDictionary = new Dictionary<ResourceTypeSO, ResourceUITemplate>();
            
            CreateUI();
        }

        private void CreateUI()
        {
            for (var index = 0; index < _resourceTypeList.List.Count; index++)
            {
                var resourceTypeSo = _resourceTypeList.List[index];

                var resourceTransform = Instantiate(_resourceUIPrefab, transform);
                var resourceUITemplate = resourceTransform.GetComponent<ResourceUITemplate>();
                resourceTransform.gameObject.SetActive(true);
                resourceTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50 + XOffsetAmount * index, 0);
                resourceUITemplate.Image.sprite = resourceTypeSo.ResourceSprite;
                resourceUITemplate.ResourceAmount.SetText("0");
                _resourceUIDictionary[resourceTypeSo] = resourceUITemplate;
            }
        }

        public void Initialize(IResourceModel resourceModel)
        {
            _resourceModel = resourceModel;
            _resourceModel.OnResourceAmountChanged += UpdateUI;
        }

        private void OnDestroy()
        {
            _resourceModel.OnResourceAmountChanged -= UpdateUI;
        }

        private void UpdateUI((ResourceTypeSO resourceType, int currentAmount) valueTuple)
        {
            var (resourceType, currentAmount) = valueTuple;
            _resourceUIDictionary[resourceType].ResourceAmount.SetText(currentAmount.ToString());
        }
    }
}