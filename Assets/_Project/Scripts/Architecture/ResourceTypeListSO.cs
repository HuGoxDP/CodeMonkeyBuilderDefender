using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    [CreateAssetMenu(menuName = "Game/Resource/Create ResourceTypeList", fileName = "ResourceTypeList", order = 0)]
    public class ResourceTypeListSO : ScriptableObject
    {
        [SerializeField] private List<ResourceTypeSO> _list;
       
        public List<ResourceTypeSO> List => _list;
    }
}