using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Architecture
{
    [CreateAssetMenu(menuName = "Game/Resource/Create ResourceType", fileName = "ResourceType", order = 0)]
    public class ResourceTypeSO : ScriptableObject
    {
        [SerializeField] private string _nameString;
        [SerializeField] private Sprite _sprite;

        public string NameString => _nameString;
        public Sprite ResourceSprite => _sprite;
    }
}