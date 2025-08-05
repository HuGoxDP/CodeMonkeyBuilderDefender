using _Project.Scripts.Architecture.ScriptableObjects;

namespace _Project.Scripts.Architecture
{
    public interface IResourceGenerator
    {
        ResourceTypeSo ResourceType { get; }
        float GetTimerNormalized { get; }
        float GetAmountGeneratedPerSecond { get; }
        void TimerTick();
    }
}