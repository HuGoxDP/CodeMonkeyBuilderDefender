using _Project.Scripts.Architecture.ScriptableObjects;

namespace _Project.Scripts.Architecture.Refactoring
{
    public interface IBuilding
    {
        BuildingTypeSo GetBuildingType { get; }
    }
}