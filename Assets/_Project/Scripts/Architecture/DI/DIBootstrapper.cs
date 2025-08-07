using System;
using _Project.Scripts.Architecture.InputReader;
using _Project.Scripts.Architecture.Interfaces;
using _Project.Scripts.Architecture.MVC.ResourceSystem;
using _Project.Scripts.Architecture.Refactoring;
using _Project.Scripts.Architecture.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Architecture.DI
{
    // ReSharper disable once InconsistentNaming
    public class DIBootstrapper : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] private bool _initializeOnAwake = true;

        [Space] [Header("ScriptableObjects")] [SerializeField]
        private BuildingTypeListSo _buildingTypeListSo;

        [SerializeField] private ResourceTypeListSo _resourceTypeListSo;

        private void Awake()
        {
            if (_initializeOnAwake)
            {
                InitializeContainer();
            }
        }

        private void OnDestroy()
        {
            DIContainer.Instance.Clear();
        }

        /// <summary>
        /// Initialize all dependencies
        /// </summary>
        public void InitializeContainer()
        {
            var container = DIContainer.Instance;

            try
            {
                // Register main services
                container.RegisterSingleton<IResourceScanner, ResourceScanner>(
                    new ResourceScanner()
                );

                // Register ScriptableObjects

                container.RegisterSingleton<IBuildingTypeProvider, BuildingTypeListSo>(_buildingTypeListSo);
                container.RegisterSingleton<IResourceTypeProvider, ResourceTypeListSo>(_resourceTypeListSo);


                // Find manager in scene tree

                var inputManager = FindFirstObjectByType<InputManager>();
                if (inputManager != null)
                {
                    container.RegisterSingleton<IInputManager, InputManager>(inputManager);
                }

                var gameResourceManager = FindFirstObjectByType<GameResourceManager>();
                if (gameResourceManager != null)
                {
                    container.RegisterSingleton<IGameResourceManager, GameResourceManager>(gameResourceManager);
                }

                /*var overlayManager = FindFirstObjectByType<BuildingsOverlaysManager>();
                if (overlayManager != null)
                {
                    container.RegisterSingleton<IHarvesterOverlayManager, BuildingsOverlaysManager>(
                        overlayManager
                    );
                }*/

                var resourceGeneratorManager = FindFirstObjectByType<ResourceGeneratorManager>();
                if (resourceGeneratorManager != null)
                {
                    container.RegisterSingleton<IResourceGeneratorManager, ResourceGeneratorManager>(
                        resourceGeneratorManager
                    );
                }

                /*container.RegisterSingleton<IOverlayController, HarvesterOverlayController>
                    (new HarvesterOverlayController(container.Resolve<IHarvesterOverlayManager>()));*/

                // Register fabrics
                container.RegisterFactory<IResourceGeneratorFactory>(() => new ResourceGeneratorFactory());
                container.RegisterFactory<IOverlayDataFactory>(() => new OverlayDataFactory());

                var overlayFactory = FindFirstObjectByType<OverlayFactory>();
                container.RegisterFactory<IOverlayFactory>(() => overlayFactory);

                Debug.Log("DIBootstrapper: Container initialized successfully");
            }
            catch (Exception ex)
            {
                Debug.LogError($"DIBootstrapper: Failed to initialize container - {ex.Message}");
            }
        }
    }
}