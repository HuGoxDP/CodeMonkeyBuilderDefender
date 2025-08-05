namespace _Project.Scripts.Architecture.Interfaces
{
    public interface IHarvesterOverlayManager
    {
        void RequestHarvesterOverlay(IResourceHarvester resourceHarvester,
            IResourceGenerator resourceGenerator);

        void ReleaseHarvesterOverlay(IResourceHarvester harvester);
    }
}