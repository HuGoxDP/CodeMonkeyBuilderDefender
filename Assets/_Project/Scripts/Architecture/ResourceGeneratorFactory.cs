using _Project.Scripts.Architecture.Interfaces;

namespace _Project.Scripts.Architecture
{
    public class ResourceGeneratorFactory : IResourceGeneratorFactory
    {
        public IResourceGenerator CreateResourceGenerator(ResourceGenerationSettings settings,
            int nearbyResourceMatches)
        {
            return new ResourceGenerator(settings, nearbyResourceMatches);
        }
    }
}