using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Architecture.Interfaces;
using _Project.Scripts.Architecture.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public class ResourceAccumulator : IResourceAccumulator
    {
        private readonly IGameResourceManager _gameResourceManager;
        private readonly object _lockObject = new object();
        private readonly Dictionary<ResourceTypeSo, int> _resourcesGathered = new Dictionary<ResourceTypeSo, int>();

        public ResourceAccumulator(IGameResourceManager gameResourceManager)
        {
            _gameResourceManager = gameResourceManager ?? throw new ArgumentNullException(nameof(gameResourceManager));
            ;
        }


        public void AccumulateResource(GameResource resource)
        {
            if (resource.ResourceType == null)
            {
                Debug.LogWarning("ResourceAccumulator: Invalid resource");
                return;
            }

            lock (_lockObject)
            {
                if (_resourcesGathered.ContainsKey(resource.ResourceType))
                {
                    _resourcesGathered[resource.ResourceType] += resource.Amount;
                }
                else
                {
                    _resourcesGathered[resource.ResourceType] = resource.Amount;
                }
            }
        }

        public void SendAccumulatedResources()
        {
            lock (_lockObject)
            {
                foreach (var kvp in _resourcesGathered.ToList())
                {
                    if (kvp.Value > 0)
                    {
                        try
                        {
                            _gameResourceManager?.AddResource(kvp.Key, kvp.Value);
                            _resourcesGathered[kvp.Key] = 0;
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"ResourceAccumulator: Failed to send {kvp.Key.name} - {ex.Message}");
                        }
                    }
                }
            }
        }

        public void ClearAccumulation()
        {
            lock (_lockObject)
            {
                _resourcesGathered.Clear();
            }
        }

        public void InitializeResourceTypes(IEnumerable<ResourceTypeSo> resourceTypes)
        {
            if (resourceTypes == null) return;

            lock (_lockObject)
            {
                foreach (var resourceType in resourceTypes)
                {
                    if (resourceType != null && !_resourcesGathered.ContainsKey(resourceType))
                    {
                        _resourcesGathered[resourceType] = 0;
                    }
                }
            }
        }
    }
}