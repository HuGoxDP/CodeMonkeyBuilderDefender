using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    [CreateAssetMenu(menuName = "Game/Building/Create BuildingTypeList", fileName = "BuildingTypeList", order = 0)]
    public class BuildingTypeListSO : ScriptableObject
    {
        [SerializeField] private List<BuildingTypeSO> _list;
        
        public List<BuildingTypeSO> List => _list;
    }
}