using UnityEngine;

namespace _Project.Scripts.Architecture.ScriptableObjects
{
    [CreateAssetMenu(
        menuName = "Game/Building/Create ResourceHarvester",
        fileName = "ResourceHarvester",
        order = 0
    )]
    public class ResourceHarvesterSo : BuildingTypeSo
    {
        [SerializeField] private ResourceGenerationData _resourceGenerationData;

        public ResourceGenerationData ResourceGenerationData => _resourceGenerationData;
    }
}