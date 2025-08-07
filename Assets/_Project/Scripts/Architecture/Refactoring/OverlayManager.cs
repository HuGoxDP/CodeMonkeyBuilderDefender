namespace _Project.Scripts.Architecture.Refactoring
{
    /*public class OverlayManager : MonoBehaviour, IOverlayManager
    {
        private Dictionary<Type, IOverlayController> _controllers;

        private IOverlayDataFactory _dataFactory;
        private void Awake()
        {
            _controllers = new Dictionary<Type, IOverlayController>();
            _dataFactory = DIContainer.Instance.Resolve<IOverlayDataFactory>();
        }

        public void RegisterController<TBuilding>(IOverlayController controller) where TBuilding : class, IBuilding
        {
            if (_controllers == null)
            {
                Debug.LogWarning($"Controllers Dictionary is null");
                return;
            }

            if (IsControllerRegistered(typeof(TBuilding)))
            {
                Debug.LogWarning($"Controller {typeof(TBuilding)} is already registered");
                return;
            }

            _controllers.Add(typeof(TBuilding), controller);
        }


        public void UnregisterController<TBuilding>()
        {
            if (_controllers == null)
            {
                Debug.LogWarning($"Controllers Dictionary is null");
                return;
            }

            if (!IsControllerRegistered(typeof(TBuilding)))
            {
                Debug.LogWarning($"Controller {typeof(TBuilding)} is not registered");
                return;
            }

            _controllers.Remove(typeof(TBuilding));
        }

        public bool IsControllerRegistered(Type  buildingType)=> _controllers?.ContainsKey(buildingType) ?? false;


        public void ShowOverlay(IBuilding building)
        {
            if (building == null)
                throw new ArgumentNullException(nameof(building));

            Type buildingType = GetBuildingType(building);

            if (!CanHandleBuilding(building))
            {
                Debug.LogWarning($"Building {building} is invalid");
                return;
            }

            if (!IsControllerRegistered(buildingType))
            {
                Debug.LogWarning($"Controller {buildingType} is not registered");
                return;
            }

            var controller = _controllers[buildingType];
            var data = _dataFactory.CreateDataForBuilding(building);
            controller.ShowOverlay(building, data);
        }

        public void HideOverlay(IBuilding building)
        {
            if (building == null)
                throw new ArgumentNullException(nameof(building));

            Type buildingType = GetBuildingType(building);

            if (!IsControllerRegistered(buildingType))
            {
                Debug.LogWarning($"Controller {buildingType} is not registered");
                return;
            }

            var controller = _controllers[buildingType];
            if (!controller.HasOverlay(building))
            {
                Debug.LogWarning($"Building {building} don't have overlay");
                return;
            }

            controller.HideOverlay(building);
        }

        public Type GetBuildingType(IBuilding building)
        {
            if (building == null)
                throw new ArgumentNullException(nameof(building));

            if (building is IResourceHarvester) return typeof(IResourceHarvester);

            throw new NotSupportedException($"Building type {building.GetType().Name} is not supported");        }

        public bool CanHandleBuilding(IBuilding building)
        {
            if (building == null)
                throw new ArgumentNullException(nameof(building));

            if (building is IOverlayOwner)
                return true;

            return false;
        }
    }*/
}