using _Project.Scripts.Architecture.InputReader;
using _Project.Scripts.Architecture.MVC.ResourceManager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Architecture.UnityServiceLocator
{
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private ResourceManager _resourceManager;
        [SerializeField] private InputManager _inputManager;
        
        private IServiceLocator _serviceLocator;
        private void Awake()
        {
            _serviceLocator = ServiceLocator.Instance;
            _serviceLocator.RegisterService<ResourceManager>(_resourceManager);
            _serviceLocator.RegisterService<IInputManager>(_inputManager);
        }
    }
}