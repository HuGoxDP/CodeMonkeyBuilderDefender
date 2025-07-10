using _Project.Scripts.Architecture.ScriptableObjects;
using _Project.Scripts.Architecture.UnityServiceLocator;

namespace _Project.Scripts.Architecture
{
    public class ResourceHarvester : Building
    {
        private ResourceGenerator _resourceGenerator;
        private IServiceLocator _serviceLocator;

        private void Awake()
        {
            _serviceLocator = ServiceLocator.Instance;
        }

        private void Start()
        {
            if (BuildingType is ResourceHarvesterSo resourceHarvester)
            {
                var timerMax = resourceHarvester.ResourceGenerationData.TimerMax;
                var resourceType = resourceHarvester.ResourceGenerationData.ResourceType;
                _resourceGenerator = new ResourceGenerator(timerMax, resourceType, _serviceLocator);
            }
        }

        private void FixedUpdate()
        {
            _resourceGenerator.GenerateResource();
        }
    }
}