using System;
using _Project.Scripts.Architecture.Interfaces;
using UnityEngine;
using UnityEngine.Pool;

namespace _Project.Scripts.Architecture.Refactoring
{
    public interface IOverlayFactory
    {
        public void Register<T>(GameObject prefab);
        public T CreateOverlay<T>();
    }


    public interface IOverlay<TOverlayData>
        where TOverlayData : IOverlayData
    {
        GameObject GameObject { get; }
        bool IsActive { get; }

        void InitializePool(IObjectPool<IOverlay<TOverlayData>> pool);
        void UpdateData(TOverlayData data);
        void Release();
    }


    // Overlay Controller
    public interface IOverlayController<TBuilding, TOverlay, TOverlayData>
        where TBuilding : IBuilding where TOverlay : IOverlay<TOverlayData> where TOverlayData : IOverlayData
    {
        void ShowOverlay(TBuilding building, TOverlayData data);
        void HideOverlay(TBuilding building);
        bool HasOverlay(TBuilding building);
        TOverlay GetOverlay(TBuilding building);
        bool TryGetOverlay(TBuilding building, out TOverlay overlay);
    }


    // Overlay Manager
    public interface IOverlayManager
    {
//        void RegisterController<TBuilding>(IOverlayController controller) where TBuilding : class, IBuilding;
        void UnregisterController<TBuilding>();
        bool IsControllerRegistered(Type buildingType);

        void ShowOverlay(IBuilding building);
        void HideOverlay(IBuilding building);

        Type GetBuildingType(IBuilding building);
        bool CanHandleBuilding(IBuilding building);
    }


    public interface IOverlayDataFactory
    {
        public IOverlayData CreateDataForBuilding(IBuilding building);
    }


    // Overlay Data
    public interface IOverlayData
    {
        Vector3 Position { get; }
    }


    public interface IHarvesterOverlayData : IOverlayData
    {
        IResourceGeneratorEvents ResourceGeneratorEvents { get; }
        IResourceGeneratorData ResourceGeneratorData { get; }
    }


    public interface IDefenceOverlayData : IOverlayData
    {
        //Todo make in future
    }


    // Overlay Owner
    public interface IOverlayOwner
    {
        Transform OverlayPosition { get; }
    }
}