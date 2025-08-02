using System;
using _Project.Scripts.Architecture.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public interface IResourceGenerator
    {
        public float GetTimerNormalized { get; }
        public float GetAmountGeneratedPerSecond { get; }
        public ResourceTypeSo ResourceType { get; }
        public event Action<GameResource> OnResourceGenerated;
        public event Action<IResourceGenerator> OnTick;

        public void TimerTick();
    }

    public interface IResourceGatherer : IResourceGenerator
    {
        public void SetNearbyResourceMatches(int nearbyResourceMatches);
    }

    public class ResourceGenerator : IResourceGatherer
    {
        private readonly ResourceGenerationData _data;

        private bool _isWorking;

        private float _timer;
        private float _timerMax = 1;

        public ResourceGenerator(ResourceGenerationData data)
        {
            _data = data;
            SetNearbyResourceMatches(0);
        }

        public event Action<GameResource> OnResourceGenerated;
        public event Action<IResourceGenerator> OnTick;
        public float GetTimerNormalized => _timerMax != 0 ? 1 - Mathf.Clamp01(_timer / _timerMax) : 0;
        public float GetAmountGeneratedPerSecond => 1 / _timerMax;
        public ResourceTypeSo ResourceType => _data?.ResourceType;

        public void SetNearbyResourceMatches(int nearbyResourceMatches)
        {
            _timerMax = (_data.TimerMax / 2f) +
                        _data.TimerMax *
                        (1 - (float)nearbyResourceMatches / _data.MaxResourceNodeCount);

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