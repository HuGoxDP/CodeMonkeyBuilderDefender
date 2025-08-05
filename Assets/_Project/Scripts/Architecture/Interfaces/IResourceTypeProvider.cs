using System.Collections.Generic;
using _Project.Scripts.Architecture.ScriptableObjects;

namespace _Project.Scripts.Architecture.Interfaces
{
    public interface IResourceTypeProvider
    {
        List<ResourceTypeSo> GetResourceTypes();
    }
}