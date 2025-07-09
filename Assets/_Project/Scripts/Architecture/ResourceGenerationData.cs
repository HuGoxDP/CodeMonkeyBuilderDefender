using System;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    [Serializable]
    public class ResourceGenerationData
    {
        [SerializeField] private float _timerMax;
        [SerializeField] private ResourceTypeSO _resourceType;

        public float TimerMax => _timerMax;
        public ResourceTypeSO ResourceType => _resourceType;
    }
}