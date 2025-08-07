using System;
using _Project.Scripts.Architecture.DI;
using _Project.Scripts.Architecture.Interfaces;
using _Project.Scripts.Architecture.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Architecture.Refactoring
{
    public class ResourceHarvester : Building, IResourceHarvester, IOverlayOwner
    {
        [SerializeField] private Transform _overlayPosition;

        private IOverlayManager _overlayManager;
        private IResourceGenerator _resourceGenerator;
        private IResourceGeneratorFactory _resourceGeneratorFactory;
        private IResourceGeneratorManager _resourceGeneratorManager;
        private IResourceScanner _resourceScanner;

        public ResourceGenerationSettings ResourceGenerationData =>
            BuildingType is ResourceHarvesterSo harvester ? harvester.ResourceGenerationSettings : null;

        private void Start()
        {
            InitializeWithDI();
        }

        private void OnDestroy()
        {
            Cleanup();
        }

        private void OnDrawGizmosSelected()
        {
            if (BuildingType is ResourceHarvesterSo resourceHarvester)
            {
                Gizmos.color = Color.orange;
                Gizmos.DrawWireSphere(
                    transform.position,
                    resourceHarvester.ResourceGenerationSettings.ResourceDetectionRange
                );
            }
        }

        public Transform OverlayPosition => _overlayPosition;
        public IResourceGenerator ResourceGenerator => _resourceGenerator;

        private void Cleanup()
        {
            _overlayManager?.HideOverlay(this);

            _resourceGenerator = null;
            _resourceGeneratorManager = null;
            _resourceGenerator = null;
            _overlayManager = null;
            _resourceScanner = null;
        }

        // ReSharper disable once InconsistentNaming
        private void InitializeWithDI()
        {
            try
            {
                var container = DIContainer.Instance;

                _resourceScanner = container.Resolve<IResourceScanner>();
                _resourceGeneratorFactory = container.Resolve<IResourceGeneratorFactory>();
                _overlayManager = container.Resolve<IOverlayManager>();
                _resourceGeneratorManager = container.Resolve<IResourceGeneratorManager>();

                InitializeHarvester();
            }
            catch (Exception ex)
            {
                Debug.LogError($"DIResourceHarvester: Failed to resolve dependencies - {ex.Message}");
            }
        }

        private void InitializeHarvester()
        {
            if (ResourceGenerationData == null)
            {
                Debug.LogError("DIResourceHarvester: BuildingData is null");
                return;
            }

            try
            {
                var nearbyResources = _resourceScanner.CountResourceMatches(
                    ResourceGenerationData.ResourceType,
                    transform.position,
                    ResourceGenerationData.ResourceDetectionRange
                );

                var clampedMatches = Mathf.Clamp(nearbyResources, 0, ResourceGenerationData.MaxResourceNodeCount);

                _resourceGenerator =
                    _resourceGeneratorFactory.CreateResourceGenerator(ResourceGenerationData, clampedMatches);
                _overlayManager.ShowOverlay(this);
                _resourceGeneratorManager.AddResourceGenerator(_resourceGenerator);
            }
            catch (Exception ex)
            {
                Debug.LogError($"DIResourceHarvester: Initialization failed - {ex.Message}");
            }
        }
    }
}