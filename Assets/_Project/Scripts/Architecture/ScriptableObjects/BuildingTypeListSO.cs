using System.Collections.Generic;
using _Project.Scripts.Architecture.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Architecture.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game/Building/Create BuildingTypeList", fileName = "BuildingTypeList", order = 0)]
    public class BuildingTypeListSo : ScriptableObject, IBuildingTypeProvider
    {
        [SerializeField] private List<BuildingTypeSo> _list;
        [SerializeField] private List<BuildingTypeSo> _uiIgnore;

        public List<BuildingTypeSo> GetBuildingTypes() => _list;
        public List<BuildingTypeSo> GetUIIgnoreTypes() => _uiIgnore;
    }
}