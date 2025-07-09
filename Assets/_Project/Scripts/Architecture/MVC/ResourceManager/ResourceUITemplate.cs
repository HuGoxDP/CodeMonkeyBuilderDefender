using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Architecture.MVC.ResourceManager
{
    public class ResourceUITemplate : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _resourceAmount;
        [SerializeField] private Image _image;

        public TextMeshProUGUI ResourceAmount => _resourceAmount;
        public Image Image => _image;
    }
}