using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Architecture.ScriptableObjects
{
    [Serializable]
    public class ResourceAmount
    {
        private int _amount;
        private ResourceTypeSo _resourceType;

        public ResourceTypeSo ResourceType => _resourceType;
        public int Amount => _amount;
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
        [field: SerializeField] public List<ResourceAmount> ResourceAmounts { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject BuildingPrefabRef { get; private set; }

        public async UniTask<Transform> InstantiateAsync(Transform parent, Vector3 position, Quaternion rotation,
            CancellationToken cancellationToken = default)
        {
            return await BuildingFactory.CreateBuildingAsync(this, position, rotation, parent, cancellationToken);
        }

        public string GetPrefabKey()
        {
            return BuildingPrefabRef.RuntimeKey.ToString();
        }
    }
}