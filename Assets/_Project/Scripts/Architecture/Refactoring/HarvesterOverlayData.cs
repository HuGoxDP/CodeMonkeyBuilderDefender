using System;
using _Project.Scripts.Architecture.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Architecture.Refactoring
{
    public class HarvesterOverlayData : IHarvesterOverlayData
    {
        public HarvesterOverlayData(Vector3 position, IResourceGeneratorEvents resourceGeneratorEvents,
            IResourceGeneratorData resourceGenerator)
        {
            Position = position;
            ResourceGeneratorEvents = resourceGeneratorEvents;
            ResourceGeneratorData = resourceGenerator;
        }

        public HarvesterOverlayData(IResourceHarvester resourceHarvester)
        {
            if (resourceHarvester == null)
                throw new ArgumentNullException(nameof(resourceHarvester));

            if (resourceHarvester is not IOverlayOwner overlayOwner)
                throw new ArgumentException(
                    $"The {nameof(resourceHarvester)} must implement {nameof(IOverlayOwner)} interface",
                    nameof(resourceHarvester)
                );

            if (resourceHarvester.ResourceGenerator is not IResourceGeneratorEvents generatorEvents)
                throw new ArgumentException(
                    $"The ResourceGenerator of {nameof(resourceHarvester)} must implement {nameof(IResourceGeneratorEvents)} interface",
                    nameof(resourceHarvester)
                );

            if (resourceHarvester.ResourceGenerator is not IResourceGeneratorData generatorData)
                throw new ArgumentException(
                    $"The ResourceGenerator of {nameof(resourceHarvester)} must implement {nameof(IResourceGeneratorData)} interface",
                    nameof(resourceHarvester)
                );

            Position = overlayOwner.OverlayPosition.position;
            ResourceGeneratorEvents = generatorEvents;
            ResourceGeneratorData = generatorData;
        }

        public Vector3 Position { get; }
        public IResourceGeneratorEvents ResourceGeneratorEvents { get; }
        public IResourceGeneratorData ResourceGeneratorData { get; }
    }
}