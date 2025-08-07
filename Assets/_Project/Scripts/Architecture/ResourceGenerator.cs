using System;
using _Project.Scripts.Architecture.Interfaces;
using _Project.Scripts.Architecture.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public class ResourceGenerator : IResourceGatherer, IResourceGenerator, IResourceGeneratorEvents,
        IResourceGeneratorData
    {
        private readonly ResourceGenerationSettings _settings;

        private bool _isWorking;
        private int _nearbyResourceMatches;

        private float _timer;
        private float _timerMax = 1;

        public ResourceGenerator(ResourceGenerationSettings settings, int nearbyResourceMatches)
        {
            _settings = settings;
            SetNearbyResourceMatches(nearbyResourceMatches);
        }

        public void SetNearbyResourceMatches(int nearbyResourceMatches)
        {
            _nearbyResourceMatches = nearbyResourceMatches;

            _timerMax = (_settings.TimerMax / 2f) +
                        _settings.TimerMax *
                        (1 - (float)nearbyResourceMatches / _settings.MaxResourceNodeCount);

            _isWorking = nearbyResourceMatches > 0;
        }

        public void TimerTick()
        {
            if (!_isWorking)
                return;

            _timer -= Time.fixedDeltaTime;
            OnTick?.Invoke(this);

            if (!(_timer <= 0))
                return;

            _timer += _timerMax;
            GatherResources();
        }

        public float GetTimerNormalized => _timerMax != 0 ? 1 - Mathf.Clamp01(_timer / _timerMax) : 0;

        public float GetAmountGeneratedPerSecond
        {
            get
            {
                if (_nearbyResourceMatches == 0)
                {
                    return 0;
                }

                var value = _settings?.ResourceGenerationCount ?? 1;
                return value / _timerMax;
            }
        }

        public ResourceTypeSo ResourceType => _settings?.ResourceType;

        public event Action<GameResource> OnResourceGenerated;
        public event Action<IResourceGeneratorData> OnTick;

        private void GatherResources()
        {
            var resource = new GameResource()
            {
                ResourceType = _settings.ResourceType,
                Amount = _settings.ResourceGenerationCount
            };

            OnResourceGenerated?.Invoke(resource);
        }
    }
}