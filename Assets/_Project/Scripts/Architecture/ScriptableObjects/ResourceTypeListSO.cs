using System.Collections.Generic;
using _Project.Scripts.Architecture.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Architecture.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game/Resource/Create ResourceTypeList", fileName = "ResourceTypeList", order = 0)]
    public class ResourceTypeListSo : ScriptableObject, IResourceTypeProvider
    {
        [SerializeField] private List<ResourceTypeSo> _list;
        public List<ResourceTypeSo> GetResourceTypes() => _list;
    }
}