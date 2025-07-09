using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public abstract class Building : MonoBehaviour
    {
        protected BuildingTypeSO BuildingType;

        public void Initialize(BuildingTypeSO buildingType)
        {
            BuildingType = buildingType;
        }
    }
}