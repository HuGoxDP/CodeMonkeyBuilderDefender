using _Project.Scripts.Architecture.Interfaces;

namespace _Project.Scripts.Architecture
{
    public class HarvesterOverlayController : IOverlayController
    {
        private readonly IHarvesterOverlayManager _harvesterOverlayManager;

        public HarvesterOverlayController(IHarvesterOverlayManager harvesterOverlayManager)
        {
            _harvesterOverlayManager = harvesterOverlayManager;
        }

        public void HideOverlay(IResourceHarvester harvester)
        {
            if (_harvesterOverlayManager != null)
            {
                _harvesterOverlayManager.ReleaseHarvesterOverlay(harvester);
            }
        }

        public void ShowOverlay(IResourceHarvester harvester, IResourceGenerator generator)
        {
            if (_harvesterOverlayManager != null)
            {
                _harvesterOverlayManager.RequestHarvesterOverlay(harvester, generator);
            }
        }
    }
}