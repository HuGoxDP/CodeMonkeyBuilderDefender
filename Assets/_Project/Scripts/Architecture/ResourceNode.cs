using _Project.Scripts.Architecture.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public class ResourceNode : MonoBehaviour
    {
        [SerializeField] private ResourceTypeSo _resourceType;

        public ResourceTypeSo ResourceType => _resourceType;
    }
}