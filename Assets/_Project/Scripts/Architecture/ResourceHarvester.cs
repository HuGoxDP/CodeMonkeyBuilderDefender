using _Project.Scripts.Architecture.MVC.BuildingSystem;
using _Project.Scripts.Architecture.ScriptableObjects;
using _Project.Scripts.Architecture.UnityServiceLocator;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public class ResourceHarvester : Building
    {
        [SerializeField] private Transform _overlayPosition;
        private BuildingsOverlaysManager _buildingsOverlaysManager;

        private HarvesterOverlay _harvesterOverlay;

        private int _nearbyResourceMatches;
        private ResourceGenerator _resourceGenerator;
        private ResourceSystemManager _resourceSystemManager;
        private IServiceLocator _serviceLocator;

        public Transform OverlayPosition => _overlayPosition;

        private void Awake()
        {
            _serviceLocator = ServiceLocator.Instance;
        }

        private void Start()
        {
            if (BuildingType is ResourceHarvesterSo resourceHarvester)
            {
                var data = resourceHarvester.ResourceGenerationData;
                var layerMask = 1 << 6;
                var colliders = Physics2D.OverlapCircleAll(transform.position, data.ResourceDetectionRange, layerMask);

                _nearbyResourceMatches = Mathf.Clamp(
                    ScanResourceMatches(data.ResourceType, colliders),
                    0,
                    data.MaxResourceNodeCount
                );
                _resourceGenerator = new ResourceGenerator(data);
                _resourceGenerator.SetNearbyResourceMatches(_nearbyResourceMatches);

                _serviceLocator.GetService<BuildingsOverlaysManager>(out _buildingsOverlaysManager);
                _harvesterOverlay = _buildingsOverlaysManager.RequestHarvesterOverlay(this, _resourceGenerator);

                _serviceLocator.GetService(out _resourceSystemManager);
                _resourceSystemManager.AddResourceGenerator(_resourceGenerator);
            }
        }

        private void OnDestroy()
        {
            _buildingsOverlaysManager.ReleaseHarvesterOverlay(_harvesterOverlay);
            _resourceSystemManager.RemoveResourceGenerator(_resourceGenerator);
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

        private int ScanResourceMatches(ResourceTypeSo matchedResource, Collider2D[] colliders)
        {
            var matches = 0;
            foreach (var scannedCollider in colliders)
            {
                if (scannedCollider.TryGetComponent<ResourceNode>(out var resourceNode) &&
                    resourceNode.ResourceType == matchedResource)
                {
                    matches++;
                }
            }

            return matches;
        }
    }
}