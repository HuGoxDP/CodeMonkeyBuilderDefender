using _Project.Scripts.Architecture.MVC.ResourceSystem;
using _Project.Scripts.Architecture.UnityServiceLocator;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public class ResourceGenerator
    {
        private readonly ResourceGenerationData _data;
        private readonly ResourceManager _resourceManager;
        private int _nearbyResourceMatches;

        private float _timer;
        private float _timerMax;

        public ResourceGenerator(ResourceGenerationData data, IServiceLocator serviceLocator)
        {
            _data = data;
            serviceLocator.GetService(out _resourceManager);
            SetNearbyResourceMatches(0);
        }

        public void SetNearbyResourceMatches(int nearbyResourceMatches)
        {
            _nearbyResourceMatches = nearbyResourceMatches;
            _timerMax = (_data.TimerMax / 2f) +
                        _data.TimerMax *
                        (1 - (float)_nearbyResourceMatches / _data.MaxResourceNodeCount);
        }

        public void GenerateResource()
        {
            if (_nearbyResourceMatches == 0)
                return;

            _timer -= Time.fixedDeltaTime;
            if (_timer <= 0)
            {
                _timer += _timerMax;
                _resourceManager.AddResource(_data.ResourceType, _data.ResourceGenerationCount);
            }
        }
    }
}