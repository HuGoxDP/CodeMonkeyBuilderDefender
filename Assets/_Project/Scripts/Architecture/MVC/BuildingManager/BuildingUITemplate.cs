using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Architecture.MVC.BuildingManager
{
    public class BuildingUITemplate : MonoBehaviour
    {
       [SerializeField] private Image _buildingImage;
       [SerializeField] private Button _button;
     
       public Button Button => _button;
       public Image BuildingImage => _buildingImage;
    }
}