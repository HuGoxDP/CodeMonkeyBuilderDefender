using System;
using _Project.Scripts.Architecture.ScriptableObjects;

namespace _Project.Scripts.Architecture.Interfaces
{
    public interface IResourceGeneratorEvents
    {
        event Action<GameResource> OnResourceGenerated;
        event Action<IResourceGenerator> OnTick;
    }
}