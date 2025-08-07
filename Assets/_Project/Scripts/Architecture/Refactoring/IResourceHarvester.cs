using _Project.Scripts.Architecture.Interfaces;

namespace _Project.Scripts.Architecture.Refactoring
{
    public interface IResourceHarvester : IBuilding
    {
        IResourceGenerator ResourceGenerator { get; }
    }
}