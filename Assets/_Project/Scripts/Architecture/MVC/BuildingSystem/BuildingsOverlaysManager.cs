using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Architecture.MVC.BuildingSystem
{
    public class BuildingsOverlaysManager : MonoBehaviour
    {
        [SerializeField] private HarvesterOverlay[] _harvesterOverlaysTEST;

        private Queue<HarvesterOverlay> _releasedHarvesterOverlays;
        // Сделать Пулом

        private void Awake()
        {
            _releasedHarvesterOverlays = new Queue<HarvesterOverlay>(_harvesterOverlaysTEST);
        }

        public HarvesterOverlay RequestHarvesterOverlay(ResourceHarvester resourceHarvester,
            ResourceGenerator resourceGenerator)
        {
            var overlay = _releasedHarvesterOverlays.Dequeue();

            overlay.gameObject.SetActive(true);
            overlay.Initialize(resourceHarvester, resourceGenerator);

            return overlay;
        }

        public void ReleaseHarvesterOverlay(HarvesterOverlay overlay)
        {
            overlay.gameObject.SetActive(false);
            overlay.transform.position = new Vector3(-10000, -10000, 0);
            overlay.Release();

            _releasedHarvesterOverlays.Enqueue(overlay);
        }
    }
}