using UnityEngine;

namespace _Project.Scripts.Architecture.Interfaces
{
    public interface IResourceHarvester
    {
        Transform OverlayPosition { get; }
        ResourceGenerationData BuildingData { get; }
    }
}