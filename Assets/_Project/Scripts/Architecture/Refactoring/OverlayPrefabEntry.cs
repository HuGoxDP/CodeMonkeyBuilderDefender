using System;
using UnityEngine;

namespace _Project.Scripts.Architecture.Refactoring
{
    [Serializable]
    public class OverlayPrefabEntry
    {
        [SerializeField] private OverlayConfigBase _config;
        [SerializeField] private GameObject _prefab;

        public Type Type => _config.OverlayType;
        public GameObject Prefab => _prefab;
    }
}