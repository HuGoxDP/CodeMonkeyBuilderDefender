using _Project.Scripts.Architecture.MVC.ResourceManager;
using _Project.Scripts.Architecture.UnityServiceLocator;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public class ResourceGenerator
    {
        private float _timer;
        private float _timerMax;
        private readonly ResourceTypeSO _resourceType;

        private readonly ResourceManager _resourceModel;
        public ResourceGenerator(float timerMax, ResourceTypeSO resourceType, IServiceLocator serviceLocator)
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
                _resourceModel.AddResource(_resourceType,1);
            }
        }
    }

  
}