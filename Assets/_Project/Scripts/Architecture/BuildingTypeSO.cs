using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    [Serializable]
    public class ResourceAmount
    {
        private ResourceTypeSO _resourceType;
        private int _amount;

        public ResourceTypeSO ResourceType => _resourceType;
        public int Amount => _amount;
    }

    [CreateAssetMenu(
        menuName = "Game/Building/Create BuildingType",
        fileName = "BuildingType",
        order = 0
    )]
    public class BuildingTypeSO : ScriptableObject
    {
        [field: SerializeField] public string NameString { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public List<ResourceAmount> ResourceAmounts { get; private set; }
        [SerializeField] protected Transform Prefab;

        public Transform Instantiate(Transform parent, Vector3 position, Quaternion rotation)
        {
            var transform = Instantiate(Prefab, position, rotation, parent);
            transform.GetComponent<Building>().Initialize(this);
            return transform;
        }
    }
}