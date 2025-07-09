using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Architecture.UnityServiceLocator
{
    public interface IServiceLocator
    {
        public static IServiceLocator Instance;
        public IServiceLocator RegisterService<TService>(TService service) where TService : class;
        public IServiceLocator GetService<TService>(out TService service) where TService : class;
        public bool HasService<TService>() where TService : class;
        public IServiceLocator UnregisterService<TService>() where TService : class;
    }
    
    public class ServiceLocator : MonoBehaviour, IServiceLocator
    {
        private static IServiceLocator _instance;
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static IServiceLocator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<ServiceLocator>();
                    if (_instance == null)
                    {
                        var go = new GameObject("ServiceLocator");
                        _instance = go.AddComponent<ServiceLocator>();
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && !ReferenceEquals(_instance, this))
            {
                Debug.LogWarning("Service Locator already exists");
                Destroy(gameObject);
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Registers a service of type TService in the service locator.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <param name="service">The service instance to register.</param>
        /// <returns>The current ServiceLocator instance for method chaining.</returns>
        public IServiceLocator RegisterService<TService>(TService service) where TService : class
        {
            var serviceType = typeof(TService);
            if (_services.TryAdd(serviceType, service))
            {
                Debug.Log($"Service {serviceType.Name} registered successfully");
                return this;
            }

            Debug.LogWarning($"Service {serviceType.Name} is already registered!");
            return this;
        }

        /// <summary>
        /// Retrieves a registered service of the specified type.
        /// </summary>
        /// <typeparam name="TService">The type of service to retrieve.</typeparam>
        /// <param name="service">When this method returns, contains the requested service instance if found; otherwise, null.</param>
        /// <returns>The current ServiceLocator instance for method chaining.</returns>
        public IServiceLocator GetService<TService>(out TService service) where TService : class
        {
            var type = typeof(TService);
            if (_services.TryGetValue(type, out var obj))
            {
                service = obj as TService;
                return this;
            }

            Debug.LogWarning($"Service {typeof(TService).Name} is not registered!");

            service = null;
            return this;
        }

        /// <summary>
        /// Determines whether a service of the specified type is registered in the service locator.
        /// </summary>
        /// <typeparam name="TService">The type of service to check for.</typeparam>
        /// <returns>true if the service is registered; otherwise, false.</returns>
        public bool HasService<TService>() where TService : class => _services.ContainsKey(typeof(TService));

        /// <summary>
        /// Unregisters a service of the specified type from the service locator.
        /// </summary>
        /// <typeparam name="TService">The type of service to unregister.</typeparam>
        /// <returns>The current ServiceLocator instance for method chaining.</returns>
        public IServiceLocator UnregisterService<TService>() where TService : class
        {
            var type = typeof(TService);
            if (_services.Remove(type))
            {
                Debug.Log($"Service {type.Name} unregistered");
            }
            else
            {
                Debug.LogWarning($"Service {type.Name} was not registered");
            }
            
            return this;
        }

        private void OnDestroy()
        {
            _instance = null;
            _services.Clear();
        }
    }
}