namespace _Project.Scripts.Architecture.Interfaces
{
    public interface IResourceGeneratorManager
    {
        public void AddResourceGenerator(IResourceGenerator generator);

        public void RemoveResourceGenerator(IResourceGenerator generator);
    }
}