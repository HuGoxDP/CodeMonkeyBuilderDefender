using _Project.Scripts.Architecture.Refactoring;
using _Project.Scripts.Architecture.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public abstract class Building : MonoBehaviour, IBuilding
    {
        [SerializeField] protected BuildingTypeSo BuildingType;
        private BuildingTypeSo _getBuildingType;

        public BuildingTypeSo GetBuildingType => BuildingType;

        public void Initialize(BuildingTypeSo buildingType)
        {
            BuildingType = buildingType;
        }
    }
}