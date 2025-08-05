using System.Collections.Generic;
using _Project.Scripts.Architecture.ScriptableObjects;

namespace _Project.Scripts.Architecture.Interfaces
{
    public interface IResourceAccumulator
    {
        void AccumulateResource(GameResource resource);
        void SendAccumulatedResources();
        void ClearAccumulation();
        void InitializeResourceTypes(IEnumerable<ResourceTypeSo> resourceTypes);
    }
}