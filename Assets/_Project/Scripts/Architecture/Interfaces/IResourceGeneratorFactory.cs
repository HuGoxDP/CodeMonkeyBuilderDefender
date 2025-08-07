namespace _Project.Scripts.Architecture.Interfaces
{
    internal interface IResourceGeneratorFactory
    {
        IResourceGenerator CreateResourceGenerator(ResourceGenerationSettings settings, int nearbyResourceMatches);
    }
}