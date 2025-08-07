using UnityEngine;

namespace _Project.Scripts.Architecture.Refactoring
{
    public class OverlayRegistrar : MonoBehaviour
    {
        [SerializeField] private GameObject _harvesterOverlayPrefab;

        private void Start()
        {
            // var factory = DIContainer.Instance.Resolve<IOverlayFactory>();
            // Type harvesterOverlayType = typeof(HarvesterOverlay);
            // factory.Register<Overlay<IHarvesterOverlayData>>(_harvesterOverlayPrefab);
        }
    }
}