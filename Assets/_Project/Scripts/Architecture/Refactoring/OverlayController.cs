using System;
using UnityEngine.Pool;

namespace _Project.Scripts.Architecture.Refactoring
{
    public class
        OverlayController<TBuilding, TOverlay, TOverlayData> : IOverlayController<TBuilding, TOverlay, TOverlayData>
        where TBuilding : IBuilding where TOverlay : class, IOverlay<TOverlayData> where TOverlayData : IOverlayData
    {
        private IObjectPool<TOverlay> _overlayPool;

        public OverlayController()
        {
            _overlayPool = new ObjectPool<TOverlay>(CreateOverlay, OnGetOverlay);
        }

        public void ShowOverlay(TBuilding building, TOverlayData data)
        {
            var overlay = _overlayPool.Get();
            overlay.UpdateData(data);
        }

        public void HideOverlay(TBuilding building)
        {
            throw new NotImplementedException();
        }

        public bool HasOverlay(TBuilding building)
        {
            throw new NotImplementedException();
        }

        public TOverlay GetOverlay(TBuilding building)
        {
            throw new NotImplementedException();
        }

        public bool TryGetOverlay(TBuilding building, out TOverlay overlay)
        {
            throw new NotImplementedException();
        }

        private void OnGetOverlay(TOverlay obj)
        {
            throw new NotImplementedException();
        }

        private TOverlay CreateOverlay()
        {
            throw new NotImplementedException();
        }
    }
}