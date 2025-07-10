using _Project.Scripts.Architecture.MVC.ResourceManager;
using _Project.Scripts.Architecture.ScriptableObjects;
using _Project.Scripts.Architecture.UnityServiceLocator;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public class ResourceGenerator
    {
        private readonly ResourceManager _resourceModel;
        private readonly ResourceTypeSo _resourceType;
        private float _timer;
        private float _timerMax;

        public ResourceGenerator(float timerMax, ResourceTypeSo resourceType, IServiceLocator serviceLocator)
        {
            _timerMax = timerMax;
            _resourceType = resourceType;

            serviceLocator.GetService(out _resourceModel);
        }

        public void GenerateResource()
        {
            _timer -= Time.fixedDeltaTime;
            if (_timer <= 0)
            {
                _timer += _timerMax;
                _resourceModel.AddResource(_resourceType, 1);
            }
        }
    }
}