namespace _Project.Scripts.Architecture.Interfaces
{
    internal interface IOverlayController
    {
        void HideOverlay(IResourceHarvester harvester);
        void ShowOverlay(IResourceHarvester harvester, IResourceGenerator generator);
    }
}