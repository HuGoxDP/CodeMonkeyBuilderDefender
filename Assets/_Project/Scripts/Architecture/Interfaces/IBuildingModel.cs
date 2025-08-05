using System;
using _Project.Scripts.Architecture.ScriptableObjects;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Architecture.Interfaces
{
    public interface IBuildingModel
    {
        event Action<Building> OnBuildingAdded;
        event Action<Building> OnBuildingRemoved;
        event Action OnBuildingError;
        event Action<string> OnError;

        void AddBuilding(Building building);
        void RemoveBuilding(Building building);
        void SetResourceManager(IGameResourceManager gameResourceManager);

        UniTask<bool> CanBuild(BuildingTypeSo buildingType, Vector3 position);
        Building GetBuildingAt(Vector3 position);
    }
}