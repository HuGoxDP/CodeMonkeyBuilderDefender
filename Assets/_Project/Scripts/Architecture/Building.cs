using _Project.Scripts.Architecture.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public abstract class Building : MonoBehaviour
    {
        protected BuildingTypeSo BuildingType;

        public void Initialize(BuildingTypeSo buildingType)
        {
            BuildingType = buildingType;
        }
    }
}