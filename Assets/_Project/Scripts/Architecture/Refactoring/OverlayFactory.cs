using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Architecture.Refactoring
{
    public class OverlayFactory : MonoBehaviour, IOverlayFactory
    {
        [SerializeField] private GameObject _parent;
        [SerializeField] private OverlayPrefabEntry[] _configs;

        private Dictionary<Type, GameObject> _overlayPrefabs;

        private void Awake()
        {
            Validate();
            _overlayPrefabs = new Dictionary<Type, GameObject>();

            foreach (var config in _configs)
            {
                var prefab = config.Prefab;
                var type = config.Type;
                if (prefab == null || type == null)
                {
                    continue;
                }

                if (!_overlayPrefabs.ContainsKey(type))
                {
                    _overlayPrefabs.TryAdd(type, prefab);
                }
            }
        }

        public T CreateOverlay<T>()
        {
            if (_overlayPrefabs.TryGetValue(typeof(T), out var prefab))
            {
                var go = Instantiate(prefab, _parent.transform);
                if (go.TryGetComponent<T>(out var overlay))
                {
                    return overlay;
                }

                throw new Exception($"Prefab {prefab.name} does not have a component of type {typeof(T)}");
            }

            throw new Exception($"There is no overlay prefab {typeof(T)}");
        }

        private void Validate()
        {
            if (_parent == null)
                throw new NullReferenceException("_parent");
        }
    }
}