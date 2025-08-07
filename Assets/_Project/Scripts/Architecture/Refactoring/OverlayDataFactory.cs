using System;
using System.Collections.Generic;

namespace _Project.Scripts.Architecture.Refactoring
{
    public class OverlayDataFactory : IOverlayDataFactory
    {
        private readonly Dictionary<Type, Func<IBuilding, IOverlayData>> _factories;

        public OverlayDataFactory()
        {
            _factories = new Dictionary<Type, Func<IBuilding, IOverlayData>>();
            InitializeFactories();
        }

        public IOverlayData CreateDataForBuilding(IBuilding building)
        {
            if (building == null)
                throw new ArgumentNullException(nameof(building));

            foreach (var (type, factory) in _factories)
            {
                if (type.IsInstanceOfType(building))
                    return factory(building);
            }

            throw new NotSupportedException($"Building type {building.GetType().Name} is not supported");
        }

        private void InitializeFactories()
        {
            _factories[typeof(IResourceHarvester)] = building
                => new HarvesterOverlayData((IResourceHarvester)building);
        }
    }
}