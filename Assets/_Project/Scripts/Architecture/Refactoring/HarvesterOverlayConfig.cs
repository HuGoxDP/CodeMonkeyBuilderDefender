using System;
using UnityEngine;

namespace _Project.Scripts.Architecture.Refactoring
{
    [CreateAssetMenu(menuName = "Game/Overlays/Harvester Overlay Config")]
    public class HarvesterOverlayConfig : OverlayConfigBase
    {
        public override Type OverlayType => typeof(HarvesterOverlay);
    }
}