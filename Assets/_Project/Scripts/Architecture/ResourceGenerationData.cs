using System;
using _Project.Scripts.Architecture.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    [Serializable]
    public class ResourceGenerationData
    {
        [SerializeField] private float _timerMax;
        [SerializeField] private ResourceTypeSo _resourceType;
        [SerializeField] private int _resourceGenerationCount;

        [SerializeField] private float _resourceDetectionRange;
        [SerializeField] private int _maxResourceNodeCount;

        public float TimerMax => _timerMax;
        public ResourceTypeSo ResourceType => _resourceType;
        public int ResourceGenerationCount => _resourceGenerationCount;

        public float ResourceDetectionRange => _resourceDetectionRange;
        public int MaxResourceNodeCount => _maxResourceNodeCount;
    }
}