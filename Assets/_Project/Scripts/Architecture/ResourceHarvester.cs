using _Project.Scripts.Architecture.ScriptableObjects;
using _Project.Scripts.Architecture.UnityServiceLocator;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public class ResourceHarvester : Building
    {
        private int _nearbyResourceMatches;
        private ResourceGenerator _resourceGenerator;
        private ResourceSystemManager _resourceSystemManager;
        private IServiceLocator _serviceLocator;

        private void Awake()
        {
            _serviceLocator = ServiceLocator.Instance;
        }

        private void Start()
        {
            if (BuildingType is ResourceHarvesterSo resourceHarvester)
            {
                var data = resourceHarvester.ResourceGenerationData;
                var colliders = Physics2D.OverlapCircleAll(transform.position, data.ResourceDetectionRange, 6);

                _nearbyResourceMatches = Mathf.Clamp(
                    ScanResourceMatches(data.ResourceType, colliders),
                    0,
                    data.MaxResourceNodeCount
                );
                _resourceGenerator = new ResourceGenerator(data, _serviceLocator);
                _resourceGenerator.SetNearbyResourceMatches(_nearbyResourceMatches);

                _serviceLocator.GetService(out _resourceSystemManager);
                _resourceSystemManager.AddResourceGenerator(_resourceGenerator);
            }
        }

        private void OnDestroy()
        {
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