using UnityEngine;
using UnityEngine.Pool;

namespace _Project.Scripts.Architecture.Interfaces
{
    public interface IHarvesterOverlay
    {
        GameObject GameObject { get; }
        void Initialize(IResourceHarvester harvester, IResourceGenerator generator);
        void InitializePool(IObjectPool<IHarvesterOverlay> pool);
        void Release();
    }
}