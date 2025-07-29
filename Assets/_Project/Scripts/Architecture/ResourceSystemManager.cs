using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public class ResourceSystemManager : MonoBehaviour
    {
        private List<ResourceGenerator> _resourceGenerators;

        private void Awake()
        {
            _resourceGenerators = new List<ResourceGenerator>();
        }

        private void FixedUpdate()
        {
            foreach (var resourceGenerator in _resourceGenerators)
            {
                resourceGenerator?.GenerateResource();
            }
        }

        public void AddResourceGenerator(ResourceGenerator resourceGenerator)
        {
            if (!_resourceGenerators.Contains(resourceGenerator))
            {
                _resourceGenerators.Add(resourceGenerator);
            }
        }

        public void RemoveResourceGenerator(ResourceGenerator resourceGenerator)
        {
            if (_resourceGenerators.Contains(resourceGenerator))
            {
                _resourceGenerators.Remove(resourceGenerator);
            }
        }
    }
}