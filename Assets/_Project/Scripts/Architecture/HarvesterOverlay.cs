using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Architecture
{
    public class HarvesterOverlay : MonoBehaviour
    {
        [SerializeField] private Image _resourceTypeImage;
        [SerializeField] private Slider _progressBar;
        [SerializeField] private TextMeshProUGUI _resourceGatheringCountText;

        private IResourceGenerator _currentResourceGenerator;

        private void OnDestroy()
        {
            Release();
        }

        public void Initialize(ResourceHarvester resourceHarvester, ResourceGenerator resourceGenerator)
        {
            if (resourceHarvester == null)
                throw new ArgumentNullException(nameof(resourceHarvester));
            if (resourceGenerator == null)
                throw new ArgumentNullException(nameof(resourceGenerator));

            transform.position = resourceHarvester.OverlayPosition.position;
            if (_currentResourceGenerator != null && _currentResourceGenerator != resourceGenerator)
            {
                Release();
            }

            _currentResourceGenerator = resourceGenerator;
            _resourceTypeImage.sprite = resourceGenerator.ResourceType.ResourceSprite;
            _resourceGatheringCountText.text = resourceGenerator.GetAmountGeneratedPerSecond.ToString("F1");
            resourceGenerator.OnTick += OnTick;
        }

        public void Release()
        {
            if (_currentResourceGenerator == null)
                return;

            _currentResourceGenerator.OnTick -= OnTick;
            _currentResourceGenerator = null;
        }

        private void OnTick(IResourceGenerator resourceGenerator)
        {
            _progressBar.value = resourceGenerator.GetTimerNormalized;
        }
    }
}