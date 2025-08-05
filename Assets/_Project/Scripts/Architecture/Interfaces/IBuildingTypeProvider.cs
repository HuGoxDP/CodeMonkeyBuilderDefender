using System.Collections.Generic;
using _Project.Scripts.Architecture.ScriptableObjects;

namespace _Project.Scripts.Architecture.Interfaces
{
    public interface IBuildingTypeProvider
    {
        List<BuildingTypeSo> GetBuildingTypes();
        List<BuildingTypeSo> GetUIIgnoreTypes();
    }
}