using System;
using System.Collections.Generic;

namespace _Project.Scripts.Architecture.Interfaces
{
    public interface IGeneratorRegistry
    {
        void AddGenerator(IResourceGenerator generator);
        void RemoveGenerator(IResourceGenerator generator);
        void TickAllGenerators();
        IReadOnlyList<IResourceGenerator> GetAllGenerators();

        event Action<IResourceGenerator> OnGeneratorAdded;
        event Action<IResourceGenerator> OnGeneratorRemoved;
    }
}