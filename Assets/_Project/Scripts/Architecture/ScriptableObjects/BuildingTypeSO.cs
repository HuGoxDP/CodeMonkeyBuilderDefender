using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Architecture.ScriptableObjects
{
    [Serializable]
    public struct GameResource
    {
        public ResourceTypeSo ResourceType;
        public int Amount;

        public GameResource(ResourceTypeSo resourceType, int amount)
        {
            ResourceType = resourceType;
            Amount = amount;
        }
    }


    public abstract class BuildingTypeSo : ScriptableObject
    {
        [field: SerializeField] public string NameString { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public List<GameResource> ResourceCostAmounts { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject BuildingPrefabRef { get; private set; }
        [field: SerializeField] public float MinConstructionRadius { get; private set; }
        [field: SerializeField] public float MaxConstructionRadius { get; private set; }

        public string GetPrefabKey()
        {
            return BuildingPrefabRef.RuntimeKey.ToString();
        }

        public GameResource[] GetBuildCosts()
        {
            return ResourceCostAmounts.ToArray();
        }
    }
}