using UnityEngine;

namespace _Project.Scripts.Architecture.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Game/Resource/Create ResourceType", fileName = "ResourceType", order = 0)]
    public class ResourceTypeSo : ScriptableObject
    {
        [SerializeField] private string _nameString;
        [SerializeField] private Sprite _sprite;

        public string NameString => _nameString;
        public Sprite ResourceSprite => _sprite;
    }
}