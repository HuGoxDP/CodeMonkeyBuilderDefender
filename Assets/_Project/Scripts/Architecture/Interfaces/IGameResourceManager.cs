using _Project.Scripts.Architecture.ScriptableObjects;

namespace _Project.Scripts.Architecture.Interfaces
{
    public interface IGameResourceManager
    {
        void AddResource(ResourceTypeSo resource, int amount);
        void RemoveResource(ResourceTypeSo resource, int amount);
    }
}