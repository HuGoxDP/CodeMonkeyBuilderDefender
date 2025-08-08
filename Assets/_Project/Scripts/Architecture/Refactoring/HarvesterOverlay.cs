using _Project.Scripts.Architecture.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Architecture.Refactoring
{
    public class HarvesterOverlay : Overlay<IHarvesterOverlayData>
    {
        [SerializeField] private Image _resourceTypeImage;
        [SerializeField] private Slider _progressBar;
        [SerializeField] private TextMeshProUGUI _resourceGatheringCountText;

        private IResourceGeneratorEvents _currentGeneratorEvents;
        private IResourceGeneratorData _currentResourceGeneratorData;

        private void Awake()
        {
            Validate();
        }


        private void Validate()
        {
            if (_resourceTypeImage == null)
                _resourceTypeImage = GetComponentInChildren<Image>();

            if (_progressBar == null)
                _progressBar = GetComponentInChildren<Slider>();

            if (_resourceGatheringCountText == null)
                _resourceGatheringCountText = GetComponentInChildren<TextMeshProUGUI>();
        }


        private void OnTick(IResourceGeneratorData resourceGeneratorData)
        {
            if (_progressBar != null && resourceGeneratorData != null)
            {
                _progressBar.value = resourceGeneratorData.GetTimerNormalized;
            }
        }


        public override void UpdateData(IHarvesterOverlayData data)
        {
            base.UpdateData(data);

            _currentGeneratorEvents = data.ResourceGeneratorEvents;
            _currentResourceGeneratorData = data.ResourceGeneratorData;

            if (_currentGeneratorEvents != null)
            {
                _currentGeneratorEvents.OnTick += OnTick;
            }

            if (_currentResourceGeneratorData != null)
            {
                _resourceTypeImage.sprite = _currentResourceGeneratorData.ResourceType.ResourceSprite;
                _resourceGatheringCountText.text =
                    _currentResourceGeneratorData.GetAmountGeneratedPerSecond.ToString("F1");
            }
        }


        public override void Release()
        {
            if (_currentGeneratorEvents != null)
            {
                _currentGeneratorEvents.OnTick -= OnTick;
                _currentGeneratorEvents = null;
            }

            _currentResourceGeneratorData = null;
        }
    }
}