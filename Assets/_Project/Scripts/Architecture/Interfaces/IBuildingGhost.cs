using _Project.Scripts.Architecture.InputReader;
using _Project.Scripts.Architecture.MVC.BuildingSystem;

namespace _Project.Scripts.Architecture.Interfaces
{
    public interface IBuildingGhost
    {
        void Initialize(BuildingManager manager, IPointerPositionInputReader pointerPositionInputReader);
    }
}