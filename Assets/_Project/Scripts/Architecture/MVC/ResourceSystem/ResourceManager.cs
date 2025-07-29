using System;
using _Project.Scripts.Architecture.ScriptableObjects;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Architecture.MVC.ResourceSystem
{
    public class ResourceManager : MonoBehaviour
    {
        [SerializeField] private ResourceView _resourceView;
        private IResourceModel _resourceModel;
        private ResourceTypeListSo _resourceTypeList;


        private void Awake()
        {
            InitializeResourceManager().Forget();
        }

        private async UniTaskVoid InitializeResourceManager()
        {
            try
            {
                _resourceTypeList = await AssetManager.Instance
                    .LoadAsset<ResourceTypeListSo>(nameof(ResourceTypeListSo));

                _resourceModel = new ResourceModel(_resourceTypeList);

                if (_resourceView != null)
                {
                    _resourceView.Initialize(_resourceModel, _resourceTypeList);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Resource Manager init failed: {e.Message}");
            }
        }

        public void AddResource(ResourceTypeSo resourceTypeSo, int amount)
        {
            _resourceModel.AddResource(resourceTypeSo, amount);
        }
    }
}