using System;
using _Project.Scripts.Architecture.DI;
using _Project.Scripts.Architecture.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public class ResourceGeneratorManager : MonoBehaviour, IResourceGeneratorManager
    {
        [SerializeField] private float _sendTimerMax = 1;

        private IResourceAccumulator _accumulator;
        private IGameResourceManager _gameResourceManager;
        private IGeneratorRegistry _generatorRegistry;
        private bool _isInitialized;

        private IResourceTypeProvider _resourceTypeProvider;

        private float _sendTimer;


        private void Awake()
        {
            InitializeWithDI();
        }

        private void FixedUpdate()
        {
            if (!_isInitialized)
                return;

            _generatorRegistry.TickAllGenerators();
            _sendTimer -= Time.fixedDeltaTime;
            if (_sendTimer <= 0)
            {
                _sendTimer += _sendTimerMax;
                _accumulator?.SendAccumulatedResources();
            }
        }

        private void OnDestroy()
        {
            ShutdownSystem();
        }

        public void AddResourceGenerator(IResourceGenerator generator)
        {
            if (!_isInitialized)
            {
                Debug.LogWarning("ResourceGeneratorManager: System not initialized");
                return;
            }

            _generatorRegistry?.AddGenerator(generator);
        }

        public void RemoveResourceGenerator(IResourceGenerator generator)
        {
            if (!_isInitialized) return;

            _generatorRegistry?.RemoveGenerator(generator);
        }

        private void ShutdownSystem()
        {
            if (!_isInitialized) return;

            try
            {
                if (_generatorRegistry != null)
                {
                    _generatorRegistry.OnGeneratorAdded -= OnGeneratorAdded;
                    _generatorRegistry.OnGeneratorRemoved -= OnGeneratorRemoved;
                }

                _accumulator?.SendAccumulatedResources();
                _accumulator?.ClearAccumulation();

                _isInitialized = false;

                Debug.Log("ResourceGeneratorManager: System shutdown completed");
            }
            catch (Exception ex)
            {
                Debug.LogError($"ResourceGeneratorManager: Shutdown failed - {ex.Message}");
            }
        }

        // ReSharper disable once InconsistentNaming
        private void InitializeWithDI()
        {
            try
            {
                var container = DIContainer.Instance;

                _resourceTypeProvider = container.Resolve<IResourceTypeProvider>();
                _gameResourceManager = container.Resolve<IGameResourceManager>();

                InitializeSystem();
            }
            catch (Exception ex)
            {
                Debug.LogError($"ResourceGeneratorManager: Failed to resolve dependencies - {ex.Message}");
                _isInitialized = false;
            }
        }

        private void InitializeSystem()
        {
            if (_gameResourceManager == null)
            {
                Debug.LogError("ResourceGeneratorManager: ResourceManager not found");
                return;
            }

            _accumulator = new ResourceAccumulator(_gameResourceManager);
            if (_resourceTypeProvider != null)
            {
                _accumulator.InitializeResourceTypes(_resourceTypeProvider.GetResourceTypes());
            }

            _generatorRegistry = new GeneratorRegistry();

            _generatorRegistry.OnGeneratorAdded += OnGeneratorAdded;
            _generatorRegistry.OnGeneratorRemoved += OnGeneratorRemoved;

            _isInitialized = true;
            Debug.Log("ResourceGeneratorManager: System initialized successfully");
        }

        private void OnGeneratorAdded(IResourceGenerator generator)
        {
            if (generator is IResourceGeneratorEvents eventGenerator)
            {
                eventGenerator.OnResourceGenerated += _accumulator.AccumulateResource;
            }
        }

        private void OnGeneratorRemoved(IResourceGenerator generator)
        {
            if (generator is IResourceGeneratorEvents eventGenerator)
            {
                eventGenerator.OnResourceGenerated -= _accumulator.AccumulateResource;
            }
        }
    }
}