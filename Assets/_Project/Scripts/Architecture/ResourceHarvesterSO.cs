using UnityEngine;

namespace _Project.Scripts.Architecture
{
    [CreateAssetMenu(
        menuName = "Game/Building/Create ResourceHarvester",
        fileName = "ResourceHarvester",
        order = 0
    )]
    public class ResourceHarvesterSO : BuildingTypeSO
    {
        [SerializeField] private ResourceGenerationData _resourceGenerationData;

        public ResourceGenerationData ResourceGenerationData => _resourceGenerationData;
    }
}