using System;
using _Project.Scripts.Architecture.Interfaces;
using _Project.Scripts.Architecture.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public class ResourceGenerator : IResourceGatherer, IResourceGenerator, IResourceGeneratorEvents
    {
        private readonly ResourceGenerationData _data;

        private bool _isWorking;
        private int _nearbyResourceMatches;

        private float _timer;
        private float _timerMax = 1;

        public ResourceGenerator(ResourceGenerationData data, int nearbyResourceMatches)
        {
            _data = data;
            SetNearbyResourceMatches(nearbyResourceMatches);
        }

        public void SetNearbyResourceMatches(int nearbyResourceMatches)
        {
            _nearbyResourceMatches = nearbyResourceMatches;

            _timerMax = (_data.TimerMax / 2f) +
                        _data.TimerMax *
                        (1 - (float)nearbyResourceMatches / _data.MaxResourceNodeCount);

            _isWorking = nearbyResourceMatches > 0;
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

                var value = _data?.ResourceGenerationCount ?? 1;
                return value / _timerMax;
            }
        }

        public ResourceTypeSo ResourceType => _data?.ResourceType;

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

        public event Action<GameResource> OnResourceGenerated;
        public event Action<IResourceGenerator> OnTick;

        private void GatherResources()
        {
            var resource = new GameResource()
            {
                ResourceType = _data.ResourceType,
                Amount = _data.ResourceGenerationCount
            };

            OnResourceGenerated?.Invoke(resource);
        }
    }
}