using System;
using System.Collections.Generic;

namespace _Project.Scripts.Architecture.DI
{
    // ReSharper disable once InconsistentNaming
    public class DIContainer
    {
        private static DIContainer _instance;
        private readonly Dictionary<Type, Func<object>> _factories = new Dictionary<Type, Func<object>>();
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static DIContainer Instance => _instance ??= new DIContainer();

        public void RegisterSingleton<TInterface, TImplementation>(TImplementation implementation)
            where TImplementation : class, TInterface
        {
            _services[typeof(TInterface)] = implementation;
        }

        public void RegisterFactory<TInterface>(Func<TInterface> factory)
        {
            _factories[typeof(TInterface)] = () => factory();
        }

        public T Resolve<T>()
        {
            var type = typeof(T);

            if (_services.TryGetValue(type, out var service))
            {
                return (T)service;
            }

            if (_factories.TryGetValue(type, out var factory))
            {
                return (T)factory();
            }

            throw new InvalidOperationException($"Service of type {type.Name} is not registered");
        }

        public bool IsRegistered<T>()
        {
            var type = typeof(T);
            return _services.ContainsKey(type) || _factories.ContainsKey(type);
        }

        public void Clear()
        {
            _services.Clear();
            _factories.Clear();
        }
    }

    // ReSharper disable once InconsistentNaming
}