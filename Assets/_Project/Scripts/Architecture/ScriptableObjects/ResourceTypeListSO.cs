using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Architecture.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game/Resource/Create ResourceTypeList", fileName = "ResourceTypeList", order = 0)]
    public class ResourceTypeListSo : ScriptableObject
    {
        [SerializeField] private List<ResourceTypeSo> _list;

        public List<ResourceTypeSo> List => _list;
    }
}