using System;
using System.Collections.Generic;
using _Project.Scripts.Architecture.MVC.ResourceSystem;
using _Project.Scripts.Architecture.ScriptableObjects;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Architecture.MVC.BuildingSystem
{
    public interface IBuildingModel
    {
        event Action<Building> OnBuildingAdded;
        event Action<Building> OnBuildingRemoved;
        event Action OnBuildingError;
        event Action<string> OnError;

        void AddBuilding(Building building);
        void RemoveBuilding(Building building);
        void SetResourceManager(ResourceManager resourceManager);

        UniTask<bool> CanBuild(BuildingTypeSo buildingType, Vector3 position);
        Building GetBuildingAt(Vector3 position);
    }

    public class BuildingModel : IBuildingModel
    {
        private readonly List<Building> _buildings = new List<Building>();
        private ResourceManager _resourceManager;
        public event Action<Building> OnBuildingAdded;
        public event Action<Building> OnBuildingRemoved;
        public event Action OnBuildingError;
        public event Action<string> OnError;

        public void AddBuilding(Building building)
        {
            if (building == null)
            {
                OnError?.Invoke($"BuildingModel.AddBuilding is null.");
                return;
            }

            //TODO Cписать ресурсы

            _buildings.Add(building);
            OnBuildingAdded?.Invoke(building);
        }

        public void RemoveBuilding(Building building)
        {
            if (building == null)
            {
                OnError?.Invoke($"BuildingModel.RemoveBuilding is null.");
                return;
            }

            if (!_buildings.Contains(building))
            {
                OnError?.Invoke($"BuildingModel.RemoveBuilding is invalid.");
                return;
            }

            _buildings.Remove(building);
            //TODO вернуть ресурсы потраченые на постройку(30%)
            OnBuildingRemoved?.Invoke(building);
        }

        public void SetResourceManager(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public async UniTask<bool> CanBuild(BuildingTypeSo buildingType, Vector3 position)
        {
            bool hasResources = HasSufficientResources(buildingType);
            bool validBuildingRadius = ValidateBuildingRadius(buildingType, position);
            bool validPosition = await IsValidPosition(buildingType, position);
            bool isCanBuild = hasResources && validPosition && validBuildingRadius;

            if (!isCanBuild)
            {
                OnBuildingError?.Invoke();
            }

            return isCanBuild;
        }

        public Building GetBuildingAt(Vector3 position)
        {
            var hit = Physics2D.Raycast(position, Vector2.zero, 1);
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<Building>(out var building))
                {
                    return building;
                }
            }

            return null;
        }

        private bool HasSufficientResources(BuildingTypeSo buildingType)
        {
            // TODO проверка хватает ли ресурсов
            return true;
        }

        private async UniTask<bool> IsValidPosition(BuildingTypeSo buildingType, Vector3 position)
        {
            Collider2D[] colliders;
            GameObject prefab = await AssetManager.Instance.LoadAsset<GameObject>(buildingType.GetPrefabKey());
            if (prefab.TryGetComponent<BoxCollider2D>(out var boxCollider2D))
            {
                colliders = Physics2D.OverlapBoxAll(
                    position + (Vector3)boxCollider2D.offset,
                    boxCollider2D.size,
                    0
                );

                if (colliders.Length != 0)
                {
                    return false;
                }
            }
            else
            {
                Debug.LogError($"BuildingModel.IsValidPosition: Prefab doesn't contain BoxCollider2D.");
            }

            colliders = Physics2D.OverlapCircleAll(position, buildingType.MinConstructionRadius);
            foreach (var collider in colliders)
            {
                if (!collider.TryGetComponent<Building>(out var building))
                    continue;

                if (building.GetBuildingType().BuildingPrefabRef == buildingType.BuildingPrefabRef)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ValidateBuildingRadius(BuildingTypeSo buildingType, Vector3 position)
        {
            Collider2D[] colliders;
            colliders = Physics2D.OverlapCircleAll(position, buildingType.MaxConstructionRadius);
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<Building>(out var building))
                {
                    return true;
                }
            }

            return false;
        }
    }
}