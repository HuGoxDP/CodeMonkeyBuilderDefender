namespace _Project.Scripts.Architecture.Refactoring
{
    /*
        public class HarvesterOverlayController : IOverlayController
        {
            private readonly IHarvesterOverlayManager _harvesterOverlayManager;

            public HarvesterOverlayController(IHarvesterOverlayManager harvesterOverlayManager)
            {
                _harvesterOverlayManager = harvesterOverlayManager;
            }

            public void HideOverlay(IBuilding building)
            {
                if (_harvesterOverlayManager != null && building is IResourceHarvester resourceHarvester)
                {
                    if (building is IOverlayOwner)
                    {
                        _harvesterOverlayManager.ReleaseHarvesterOverlay(resourceHarvester);

                    }
                }
            }

            public void ShowOverlay(IBuilding building)
            {
                if (_harvesterOverlayManager != null && building is IResourceHarvester resourceHarvester)
                {
                    if (building is IOverlayOwner)
                    {
                       _harvesterOverlayManager.RequestHarvesterOverlay(resourceHarvester, resourceHarvester.ResourceGenerator);
                    }
                }
            }
        }*/
}