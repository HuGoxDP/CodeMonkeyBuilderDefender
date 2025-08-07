using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Architecture.Refactoring
{
    public class OverlayFactory : MonoBehaviour, IOverlayFactory
    {
        [SerializeField] private GameObject _parent;
        private Dictionary<Type, GameObject> _overlayDictionary;

        private void Awake()
        {
            _overlayDictionary = new Dictionary<Type, GameObject>();
        }

        public void Register<T>(GameObject prefab)
        {
            Debug.Log($"Registering overlay {typeof(T).Name}");
            if (_overlayDictionary != null)
                _overlayDictionary[typeof(T)] = prefab;
        }

        public T CreateOverlay<T>()
        {
            /*
            var prefab = _overlayDictionary[typeof(T)];
            var go = Instantiate(prefab, _parent.transform);
            if (go.TryGetComponent<T>(out var overlay))
            {
                return overlay;
            }
            */

            throw new Exception($"Unable to create overlay of type {typeof(T).Name}");
        }
    }
}