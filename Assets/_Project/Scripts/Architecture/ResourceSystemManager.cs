using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Architecture.MVC.ResourceSystem;
using _Project.Scripts.Architecture.ScriptableObjects;
using _Project.Scripts.Architecture.UnityServiceLocator;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public class ResourceSystemManager : MonoBehaviour
    {
        [SerializeField] private float _sendTimerMax = 1;

        private readonly List<ResourceGenerator> _resourceGenerators = new List<ResourceGenerator>();
        private readonly Dictionary<ResourceTypeSo, int> _resourcesGathered = new Dictionary<ResourceTypeSo, int>();

        private bool _isInitialized;
        private ResourceManager _resourceManager;

        private ResourceTypeListSo _resourceTypeList;
        private float _sendTimer;

        private void Awake()
        {
            Initialize().Forget();
        }

        private void FixedUpdate()
        {
            if (!_isInitialized)
                return;

            TimerTick();

            foreach (var resourceGenerator in _resourceGenerators)
            {
                resourceGenerator?.TimerTick();
            }
        }

        private void OnDestroy()
        {
            _isInitialized = false;

            var generators = _resourceGenerators.ToArray();
            foreach (var resourceGenerator in generators)
            {
                if (resourceGenerator != null)
                {
                    resourceGenerator.OnResourceGenerated -= GatherResource;
                }
            }

            _resourceGenerators.Clear();
            _resourcesGathered.Clear();
        }

        private void TimerTick()
        {
            _sendTimer -= Time.fixedDeltaTime;

            if (!(_sendTimer <= 0))
                return;

            _sendTimer += _sendTimerMax;
            SendResources();
        }

        private void SendResources()
        {
            foreach (var kvp in _resourcesGathered.ToList())
            {
                if (kvp.Value > 0)
                {
                    _resourceManager?.AddResource(kvp.Key, kvp.Value);
                    _resourcesGathered[kvp.Key] = 0;
                }
            }
        }

        private async UniTaskVoid Initialize()
        {
            try
            {
                ServiceLocator.Instance.GetService(out _resourceManager);
                if (_resourceManager == null)
                    Debug.LogError("ResourceSystemManager:Initialize Resource Manager not found");

                _resourceTypeList =
                    await Resources.LoadAsync<ResourceTypeListSo>(nameof(ResourceTypeListSo)).ToUniTask() as
                        ResourceTypeListSo;


                if (_resourceTypeList == null)
                {
                    Debug.LogError("ResourceSystemManager:Initialize _resourceTypeList not found");
                }

                if (_resourceTypeList != null)
                {
                    foreach (var resourceType in _resourceTypeList.List)
                    {
                        _resourcesGathered[resourceType] = 0;
                    }
                }

                _isInitialized = true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"ResourceSystemManager initialization failed: {ex.Message}");
                _isInitialized = false;
            }
        }

        public void AddResourceGenerator(ResourceGenerator resourceGenerator)
        {
            if (resourceGenerator == null)
            {
                Debug.LogWarning("Trying to add null resource generator");
                return;
            }

            if (!_resourceGenerators.Contains(resourceGenerator))
            {
                _resourceGenerators.Add(resourceGenerator);
                resourceGenerator.OnResourceGenerated += GatherResource;
            }
        }

        private void GatherResource(GameResource resource)
        {
            if (resource.ResourceType == null)
                return;

            if (_resourcesGathered.ContainsKey(resource.ResourceType))
            {
                _resourcesGathered[resource.ResourceType] += resource.Amount;
            }
        }

        public void RemoveResourceGenerator(ResourceGenerator resourceGenerator)
        {
            if (_resourceGenerators.Contains(resourceGenerator))
            {
                _resourceGenerators.Remove(resourceGenerator);
                resourceGenerator.OnResourceGenerated -= GatherResource;
            }
        }
    }
}