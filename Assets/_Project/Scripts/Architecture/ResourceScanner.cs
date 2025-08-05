using System.Collections.Generic;
using _Project.Scripts.Architecture.Interfaces;
using _Project.Scripts.Architecture.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public class ResourceScanner : IResourceScanner
    {
        private const int ResourceLayerMask = 1 << 6;

        public int CountResourceMatches(ResourceTypeSo resourceType, Vector3 position, float range)
        {
            var colliders = Physics2D.OverlapCircleAll(position, range, ResourceLayerMask);

            var matches = 0;
            foreach (var scannedCollider in colliders)
            {
                if (scannedCollider.TryGetComponent<ResourceNode>(out var resourceNode) &&
                    resourceNode.ResourceType == resourceType)
                {
                    matches++;
                }
            }

            return matches;
        }

        public Collider2D[] FindResourceMatches(ResourceTypeSo resourceType, Vector3 position, float range)
        {
            var colliders = Physics2D.OverlapCircleAll(position, range, ResourceLayerMask);

            var results = new List<Collider2D>();
            foreach (var scannedCollider in colliders)
            {
                if (scannedCollider.TryGetComponent<ResourceNode>(out var resourceNode) &&
                    resourceNode.ResourceType == resourceType)
                {
                    results.Add(scannedCollider);
                }
            }

            return results.ToArray();
        }
    }
}