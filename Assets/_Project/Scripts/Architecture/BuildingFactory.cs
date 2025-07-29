using System;
using System.Threading;
using _Project.Scripts.Architecture.ScriptableObjects;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public static class BuildingFactory
    {
        public static async UniTask<Building> CreateBuildingAsync(
            BuildingTypeSo buildingType,
            Vector3 position,
            Quaternion rotation,
            Transform parent = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var prefabKey = buildingType.GetPrefabKey();

                var instance = await AssetManager.Instance.InstantiatePrefab(
                    prefabKey,
                    position,
                    rotation,
                    parent
                ).AttachExternalCancellation(cancellationToken);

                instance.name = buildingType.NameString;

                if (instance.TryGetComponent(out Building building))
                {
                    building.Initialize(buildingType);
                    return building;
                }

                Debug.LogError($"Building component missing on {instance.name}");
                return null;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to instantiate building: {e.Message}");
                return null;
            }
        }
    }
}