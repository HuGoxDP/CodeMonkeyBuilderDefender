using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Architecture.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public class GeneratorRegistry : IGeneratorRegistry
    {
        private readonly List<IResourceGenerator> _generators = new List<IResourceGenerator>();
        private readonly object _lockObject = new object();

        public IReadOnlyList<IResourceGenerator> GetAllGenerators()
        {
            lock (_lockObject)
            {
                return _generators.ToList().AsReadOnly();
            }
        }

        public event Action<IResourceGenerator> OnGeneratorAdded;
        public event Action<IResourceGenerator> OnGeneratorRemoved;

        public void AddGenerator(IResourceGenerator generator)
        {
            if (generator == null)
            {
                Debug.LogWarning("GeneratorRegistry: Trying to add null generator");
                return;
            }

            lock (_lockObject)
            {
                if (!_generators.Contains(generator))
                {
                    _generators.Add(generator);
                    OnGeneratorAdded?.Invoke(generator);
                }
            }
        }

        public void RemoveGenerator(IResourceGenerator generator)
        {
            if (generator == null)
                return;

            lock (_lockObject)
            {
                if (_generators.Remove(generator))
                {
                    OnGeneratorRemoved?.Invoke(generator);
                }
            }
        }

        public void TickAllGenerators()
        {
            List<IResourceGenerator> generatorsCopy;
            lock (_lockObject)
            {
                generatorsCopy = _generators.ToList();
            }

            foreach (var generator in generatorsCopy)
            {
                try
                {
                    generator?.TimerTick();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"GeneratorRegistry: Error ticking generator - {ex.Message}");
                }
            }
        }
    }
}