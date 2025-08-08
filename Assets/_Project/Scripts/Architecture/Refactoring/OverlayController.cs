using System;
using System.Collections.Generic;
using _Project.Scripts.Architecture.DI;
using UnityEngine;
using UnityEngine.Pool;

namespace _Project.Scripts.Architecture.Refactoring
{
    public class
        OverlayController<TBuilding, TOverlay, TOverlayData> : IOverlayController<TBuilding, TOverlay, TOverlayData>
        where TBuilding : IBuilding where TOverlay : class, IOverlay<TOverlayData> where TOverlayData : IOverlayData
    {
        private readonly Dictionary<TBuilding, TOverlay> _buildingToOverlay;
        private readonly IOverlayFactory _overlayFactory;
        private readonly IObjectPool<TOverlay> _overlayPool;

        public OverlayController()
        {
            _overlayPool = new ObjectPool<TOverlay>(CreateOverlay, OnGetOverlay, OnReleaseOverlay);
            _buildingToOverlay = new Dictionary<TBuilding, TOverlay>();
            _overlayFactory = DIContainer.Instance.Resolve<IOverlayFactory>();
        }

        public void ShowOverlay(TBuilding building, TOverlayData data)
        {
            if (building == null)
                throw new ArgumentNullException(nameof(building));
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (HasOverlay(building))
            {
                Debug.LogWarning($"Building {building} has already been overlayed.");
                return;
            }

            var overlay = _overlayPool.Get();
            overlay.UpdateData(data);
            _buildingToOverlay.Add(building, overlay);
        }

        public void HideOverlay(TBuilding building)
        {
            if (building == null)
                throw new ArgumentNullException(nameof(building));

            if (!_buildingToOverlay.TryGetValue(building, out var overlay))
            {
                Debug.LogWarning($"Building {building} has not been overlayed.");
                return;
            }

            _overlayPool.Release(overlay);
            _buildingToOverlay.Remove(building);
        }

        public bool HasOverlay(TBuilding building)
        {
            if (building == null)
                throw new ArgumentNullException(nameof(building));

            return _buildingToOverlay.ContainsKey(building);
        }

        public TOverlay GetOverlay(TBuilding building)
        {
            if (building == null)
                throw new ArgumentNullException(nameof(building));

            if (HasOverlay(building))
            {
                return _buildingToOverlay[building];
            }

            Debug.LogWarning($"There is no overlay for building {building}");
            return null;
        }

        public bool TryGetOverlay(TBuilding building, out TOverlay overlay)
        {
            if (building == null)
                throw new ArgumentNullException(nameof(building));

            overlay = GetOverlay(building);
            return overlay != null;
        }

        private void OnReleaseOverlay(TOverlay overlay)
        {
            overlay.Release();
            overlay.GameObject.SetActive(false);
        }

        private void OnGetOverlay(TOverlay overlay)
        {
            overlay.GameObject.SetActive(true);
        }

        private TOverlay CreateOverlay()
        {
            return _overlayFactory.CreateOverlay<TOverlay>();
        }
    }
}