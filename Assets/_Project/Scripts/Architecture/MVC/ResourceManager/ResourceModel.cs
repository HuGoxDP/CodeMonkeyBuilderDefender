using System;
using System.Collections.Generic;
using System.Text;

namespace _Project.Scripts.Architecture.MVC.ResourceManager
{
    public interface IResourceModel
    {
        public event Action<(ResourceTypeSO resourceType, int currentAmount)> OnResourceAmountChanged;
        public void AddResource(ResourceTypeSO resourceTypeSo, int amount);
    }

    public class ResourceModel : IResourceModel
    {
        public event Action<(ResourceTypeSO resourceType, int currentAmount)> OnResourceAmountChanged;

        private readonly Dictionary<ResourceTypeSO, int> _resourceAmountDictionary;

        public ResourceModel(ResourceTypeListSO resourceTypeList)
        {
            _resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();

            foreach (var resourceTypeSo in resourceTypeList.List)
            {
                _resourceAmountDictionary[resourceTypeSo] = 0;
            }
        }

        public void AddResource(ResourceTypeSO resourceTypeSo, int amount)
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