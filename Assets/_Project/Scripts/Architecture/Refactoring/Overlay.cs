using UnityEngine;

namespace _Project.Scripts.Architecture.Refactoring
{
    public abstract class Overlay<TOverlayData> : MonoBehaviour, IOverlay<TOverlayData>
        where TOverlayData : IOverlayData
    {
        protected virtual void OnDestroy()
        {
            Release();
        }

        public GameObject GameObject => gameObject;
        public bool IsActive => gameObject.activeInHierarchy;

        public virtual void UpdateData(TOverlayData data)
        {
            Release();
            transform.position = data.Position;
        }

        public abstract void Release();
    }
}