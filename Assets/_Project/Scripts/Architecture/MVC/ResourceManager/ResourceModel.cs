using System;
using System.Collections.Generic;
using System.Text;
using _Project.Scripts.Architecture.ScriptableObjects;

namespace _Project.Scripts.Architecture.MVC.ResourceManager
{
    public interface IResourceModel
    {
        public event Action<(ResourceTypeSo resourceType, int currentAmount)> OnResourceAmountChanged;
        public void AddResource(ResourceTypeSo resourceTypeSo, int amount);
    }

    public class ResourceModel : IResourceModel
    {
        private readonly Dictionary<ResourceTypeSo, int> _resourceAmountDictionary;

        public ResourceModel(ResourceTypeListSo resourceTypeList)
        {
            _resourceAmountDictionary = new Dictionary<ResourceTypeSo, int>();

            foreach (var resourceTypeSo in resourceTypeList.List)
            {
                _resourceAmountDictionary[resourceTypeSo] = 0;
            }
        }

        public event Action<(ResourceTypeSo resourceType, int currentAmount)> OnResourceAmountChanged;

        public void AddResource(ResourceTypeSo resourceTypeSo, int amount)
        {
            _resourceAmountDictionary[resourceTypeSo] += amount;
            OnResourceAmountChanged?.Invoke((resourceTypeSo, _resourceAmountDictionary[resourceTypeSo]));
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var resourceTypeSo in _resourceAmountDictionary.Keys)
            {
                stringBuilder.AppendLine($"{resourceTypeSo}: {_resourceAmountDictionary[resourceTypeSo]}");
            }

            return stringBuilder.ToString();
        }
    }
}