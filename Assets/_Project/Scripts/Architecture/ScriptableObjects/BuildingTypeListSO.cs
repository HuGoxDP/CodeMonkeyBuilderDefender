using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Architecture.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game/Building/Create BuildingTypeList", fileName = "BuildingTypeList", order = 0)]
    public class BuildingTypeListSo : ScriptableObject
    {
        [SerializeField] private List<BuildingTypeSo> _list;

        public List<BuildingTypeSo> List => _list;
    }
}