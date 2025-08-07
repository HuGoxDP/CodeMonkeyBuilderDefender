namespace _Project.Scripts.Architecture.Refactoring
{
    /*
        public class BuildingsOverlaysManager : MonoBehaviour, IHarvesterOverlayManager
        {
            [SerializeField] private GameObject _harvesterOverlayPrefab;
            [SerializeField] private Transform _harvesterOverlayParent;

            private IObjectPool<IHarvesterOverlay> _harvesterOverlayPool;
            private Dictionary<IResourceHarvester, IHarvesterOverlay> _harvesterToOverlay;

            private void Awake()
            {
                _harvesterToOverlay = new Dictionary<IResourceHarvester, IHarvesterOverlay>();

                _harvesterOverlayPool = new ObjectPool<IHarvesterOverlay>(
                    CreateHarvesterOverlay,
                    ActionOnGet,
                    ActionOnRelease
                );

                void ActionOnGet(IHarvesterOverlay overlay)
                {
                    overlay.GameObject.SetActive(true);
                    overlay.InitializePool(_harvesterOverlayPool);
                }
            }

            private void Start()
            {
                InvokeRepeating(nameof(CleanDeadLinks), 15, 30);
            }

            private void OnDestroy()
            {
                if (IsInvoking())
                    CancelInvoke();
            }

            public void RequestHarvesterOverlay(IResourceHarvester resourceHarvester,
                IResourceGenerator resourceGenerator)
            {
                var overlay = _harvesterOverlayPool.Get();
                overlay.Initialize(resourceHarvester, resourceGenerator);
                _harvesterToOverlay[resourceHarvester] = overlay;
            }

            public void ReleaseHarvesterOverlay(IResourceHarvester harvester)
            {
                if (harvester is Building building &&
                    (building == null || building.gameObject == null))
                {
                    _harvesterToOverlay.Remove(harvester);
                    return;
                }

                if (_harvesterToOverlay.Remove(harvester, out var overlay))
                {
                    overlay.Release();
                }
            }

            private void CleanDeadLinks()
            {
                var keysToRemove = new List<IResourceHarvester>();

                foreach (var kvp in _harvesterToOverlay)
                {
                    var harvester = kvp.Key;
                    if (harvester is Building building)
                    {
                        if (building == null || building.gameObject == null)
                        {
                            keysToRemove.Add(harvester);
                        }
                    }
                }

                foreach (var key in keysToRemove)
                {
                    if (_harvesterToOverlay.TryGetValue(key, out var overlay))
                    {
                        overlay.Release();
                        _harvesterToOverlay.Remove(key);
                    }
                }
            }

            private void ActionOnRelease(IHarvesterOverlay overlay)
            {
                if (overlay.GameObject != null)
                {
                    overlay.GameObject.SetActive(false);
                    overlay.GameObject.transform.position = new Vector3(-10000, -10000, 0);
                }
            }

            private IHarvesterOverlay CreateHarvesterOverlay()
            {
                // TODO Factory
                var overlayObject = Instantiate(_harvesterOverlayPrefab, _harvesterOverlayParent, true);
                overlayObject.SetActive(false);

                return overlayObject.GetComponent<HarvesterOverlay>();
            }
        }*/
}