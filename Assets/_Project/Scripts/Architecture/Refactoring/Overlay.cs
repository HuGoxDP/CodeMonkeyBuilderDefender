using System;
using UnityEngine;
using UnityEngine.Pool;

namespace _Project.Scripts.Architecture.Refactoring
{
    public abstract class Overlay<TOverlayData> : MonoBehaviour, IOverlay<TOverlayData>
        where TOverlayData : IOverlayData
    {
        private IObjectPool<IOverlay<TOverlayData>> _overlayPool;

        protected virtual void OnDestroy()
        {
            if (_overlayPool != null)
            {
                var pool = _overlayPool;
                _overlayPool = null;
                pool.Release(this);
            }
        }

        public GameObject GameObject => gameObject;
        public bool IsActive => gameObject.activeInHierarchy;

        public void InitializePool(IObjectPool<IOverlay<TOverlayData>> pool)
        {
            _overlayPool = pool ?? throw new ArgumentNullException(nameof(pool));
        }

        public virtual void UpdateData(TOverlayData data)
        {
            transform.position = data.Position;
        }

        public virtual void Release()
        {
            _overlayPool.Release(this);
        }
    }
}