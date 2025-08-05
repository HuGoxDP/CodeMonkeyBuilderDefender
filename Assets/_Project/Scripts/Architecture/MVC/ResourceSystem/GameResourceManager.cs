using System;
using _Project.Scripts.Architecture.DI;
using _Project.Scripts.Architecture.Interfaces;
using _Project.Scripts.Architecture.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Architecture.MVC.ResourceSystem
{
    public class GameResourceManager : MonoBehaviour, IGameResourceManager
    {
        [SerializeField] private ResourceView _resourceView;

        private IResourceModel _resourceModel;
        private IResourceTypeProvider _resourceTypeProvider;

        private void Start()
        {
            InitializeWithDI();
        }

        public void AddResource(ResourceTypeSo resource, int amount)
        {
            _resourceModel?.AddResource(resource, amount);
        }

        public void RemoveResource(ResourceTypeSo resource, int amount)
        {
            _resourceModel?.SpendResource(resource, amount);
        }

        // ReSharper disable once InconsistentNaming
        private void InitializeWithDI()
        {
            try
            {
                var container = DIContainer.Instance;
                _resourceTypeProvider = container.Resolve<IResourceTypeProvider>();

                InitializeResourceManager();
            }
            catch (Exception ex)
            {
                Debug.LogError($"DIResourceHarvester: Failed to resolve dependencies - {ex.Message}");
            }
        }

        private void InitializeResourceManager()
        {
            _resourceModel = new ResourceModel(_resourceTypeProvider);
            _resourceView.Initialize(_resourceModel, _resourceTypeProvider);
        }
    }
}