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

        public float TimerMax => _timerMax;
        public ResourceTypeSo ResourceType => _resourceType;
    }
}