using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Architecture.MVC.BuildingSystem
{
    public class BuildingUITemplate : MonoBehaviour
    {
        [SerializeField] private Image _buildingImage;
        [SerializeField] private Button _button;
        [SerializeField] private Image _selectedImage;
        public Button Button => _button;
        public Image BuildingImage => _buildingImage;

        public void Select()
        {
            _selectedImage.gameObject.SetActive(true);
        }

        public void Deselect()
        {
            _selectedImage.gameObject.SetActive(false);
        }
    }
}