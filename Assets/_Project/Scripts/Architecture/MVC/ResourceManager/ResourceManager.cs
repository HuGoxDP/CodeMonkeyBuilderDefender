using UnityEngine;

// ReSharper disable InconsistentNaming

namespace _Project.Scripts.Architecture.MVC.ResourceManager
{
    public class ResourceManager : MonoBehaviour
    {
        [SerializeField] private ResourceView _resourceView;
        private IResourceModel _resourceModel;
        private ResourceTypeListSO _resourceTypeList;

        private void Awake()
        {
            _resourceTypeList = Resources.Load<ResourceTypeListSO>(nameof(ResourceTypeListSO));
            _resourceModel = new ResourceModel(_resourceTypeList);
            _resourceView.Initialize(_resourceModel);
        }

        public void AddResource(ResourceTypeSO resourceTypeSo, int amount)
        {
            _resourceModel.AddResource(resourceTypeSo, amount);
        }
    }
}