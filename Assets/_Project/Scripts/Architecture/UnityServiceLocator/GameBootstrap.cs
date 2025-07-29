using _Project.Scripts.Architecture.InputReader;
using _Project.Scripts.Architecture.MVC.BuildingSystem;
using _Project.Scripts.Architecture.MVC.ResourceSystem;
using UnityEngine;

namespace _Project.Scripts.Architecture.UnityServiceLocator
{
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private ResourceManager _resourceManager;
        [SerializeField] private InputManager _inputManager;

        [SerializeField] private BuildingGhost _buildingGhost;
        [SerializeField] private ResourceSystemManager _resourceSystemManager;
        private IServiceLocator _serviceLocator;

        private void Awake()
        {
            Debug.Log("GameBootstrap Awake");
            _serviceLocator = ServiceLocator.Instance;
            _serviceLocator.RegisterService<ResourceManager>(_resourceManager);
            _serviceLocator.RegisterService<IInputManager>(_inputManager);
            _serviceLocator.RegisterService<BuildingGhost>(_buildingGhost);
            _serviceLocator.RegisterService<ResourceSystemManager>(_resourceSystemManager);
        }
    }
}