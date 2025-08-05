using System;
using _Project.Scripts.Architecture.DI;
using _Project.Scripts.Architecture.Interfaces;
using _Project.Scripts.Architecture.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public class ResourceHarvester : Building, IResourceHarvester
    {
        [SerializeField] private Transform _overlayPosition;
        private IOverlayController _overlayController;
        private IResourceGenerator _resourceGenerator;
        private IResourceGeneratorFactory _resourceGeneratorFactory;
        private IResourceGeneratorManager _resourceGeneratorManager;

        private IResourceScanner _resourceScanner;

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
                    resourceHarvester.ResourceGenerationData.ResourceDetectionRange
                );
            }
        }

        public Transform OverlayPosition => _overlayPosition;

        public ResourceGenerationData BuildingData =>
            BuildingType is ResourceHarvesterSo harvester ? harvester.ResourceGenerationData : null;

        private void Cleanup()
        {
            _overlayController.HideOverlay(this);

            _resourceGenerator = null;
            _resourceGeneratorManager = null;
            _resourceGenerator = null;
            _overlayController = null;
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
                _overlayController = container.Resolve<IOverlayController>();
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
            if (BuildingData == null)
            {
                Debug.LogError("DIResourceHarvester: BuildingData is null");
                return;
            }

            try
            {
                var nearbyResources = _resourceScanner.CountResourceMatches(
                    BuildingData.ResourceType,
                    transform.position,
                    BuildingData.ResourceDetectionRange
                );

                var clampedMatches = Mathf.Clamp(nearbyResources, 0, BuildingData.MaxResourceNodeCount);

                _resourceGenerator = _resourceGeneratorFactory.CreateResourceGenerator(BuildingData, clampedMatches);
                _overlayController.ShowOverlay(this, _resourceGenerator);
                _resourceGeneratorManager.AddResourceGenerator(_resourceGenerator);
            }
            catch (Exception ex)
            {
                Debug.LogError($"DIResourceHarvester: Initialization failed - {ex.Message}");
            }
        }
    }
}