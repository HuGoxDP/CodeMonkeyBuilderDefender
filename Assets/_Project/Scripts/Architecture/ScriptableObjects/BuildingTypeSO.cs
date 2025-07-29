using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Architecture.ScriptableObjects
{
    [Serializable]
    public struct ResourceCost
    {
        public ResourceTypeSo ResourceType;
        public int Amount;

        public ResourceCost(ResourceTypeSo resourceType, int amount)
        {
            ResourceType = resourceType;
            Amount = amount;
        }
    }

    [CreateAssetMenu(
        menuName = "Game/Building/Create BuildingType",
        fileName = "BuildingType",
        order = 0
    )]
    public class BuildingTypeSo : ScriptableObject
    {
        [field: SerializeField] public string NameString { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public List<ResourceCost> ResourceAmounts { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject BuildingPrefabRef { get; private set; }
        [field: SerializeField] public float MinConstructionRadius { get; private set; }
        [field: SerializeField] public float MaxConstructionRadius { get; private set; }

        public string GetPrefabKey()
        {
            return BuildingPrefabRef.RuntimeKey.ToString();
        }

        public ResourceCost[] GetBuildCosts()
        {
            return ResourceAmounts.ToArray();
        }
    }
}