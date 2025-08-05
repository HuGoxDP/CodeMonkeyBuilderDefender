using _Project.Scripts.Architecture.Interfaces;

namespace _Project.Scripts.Architecture
{
    public class ResourceGeneratorFactory : IResourceGeneratorFactory
    {
        public IResourceGenerator CreateResourceGenerator(ResourceGenerationData data, int nearbyResourceMatches)
        {
            return new ResourceGenerator(data, nearbyResourceMatches);
        }
    }
}