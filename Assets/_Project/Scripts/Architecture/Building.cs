using _Project.Scripts.Architecture.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public abstract class Building : MonoBehaviour
    {
        [SerializeField] protected BuildingTypeSo BuildingType;

        public BuildingTypeSo GetBuildingType() => BuildingType;

        public void Initialize(BuildingTypeSo buildingType)
        {
            BuildingType = buildingType;
        }
    }
}