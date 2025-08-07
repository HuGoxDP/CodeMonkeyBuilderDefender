using _Project.Scripts.Architecture.ScriptableObjects;

namespace _Project.Scripts.Architecture.Interfaces
{
    public interface IResourceGenerator
    {
        void TimerTick();
    }

    public interface IResourceGeneratorData
    {
        ResourceTypeSo ResourceType { get; }
        float GetTimerNormalized { get; }
        float GetAmountGeneratedPerSecond { get; }
    }
}