using System;
using _Project.Scripts.Architecture.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace _Project.Scripts.Architecture
{
    public class HarvesterOverlay : MonoBehaviour, IHarvesterOverlay
    {
        [SerializeField] private Image _resourceTypeImage;
        [SerializeField] private Slider _progressBar;
        [SerializeField] private TextMeshProUGUI _resourceGatheringCountText;
        private IResourceGeneratorEvents _currentGeneratorEvents;

        private IResourceGenerator _currentResourceGenerator;

        private IObjectPool<IHarvesterOverlay> _harvesterOverlayPool;

        private void OnDestroy()
        {
            Release();
        }

        private void OnValidate()
        {
            if (_resourceTypeImage == null)
                _resourceTypeImage = GetComponentInChildren<Image>();

            if (_progressBar == null)
                _progressBar = GetComponentInChildren<Slider>();

            if (_resourceGatheringCountText == null)
                _resourceGatheringCountText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public GameObject GameObject => gameObject;

        public void InitializePool(IObjectPool<IHarvesterOverlay> pool)
        {
            _harvesterOverlayPool = pool;
        }

        public void Initialize(IResourceHarvester harvester, IResourceGenerator generator)
        {
            if (harvester == null)
                throw new ArgumentNullException(nameof(harvester));
            if (generator == null)
                throw new ArgumentNullException(nameof(generator));

            Release();

            if (harvester.OverlayPosition != null)
            {
                transform.position = harvester.OverlayPosition.position;
            }

            _currentResourceGenerator = generator;

            UpdateUI();

            if (generator is IResourceGeneratorEvents eventGenerator)
            {
                _currentGeneratorEvents = eventGenerator;
                _currentGeneratorEvents.OnTick += OnTick;
            }
        }


        public void Release()
        {
            if (_currentGeneratorEvents != null)
            {
                _currentGeneratorEvents.OnTick -= OnTick;
                _currentGeneratorEvents = null;
            }

            _currentResourceGenerator = null;

            if (_harvesterOverlayPool != null)
            {
                var pool = _harvesterOverlayPool;
                _harvesterOverlayPool = null;
                pool.Release(this);
            }
        }

        private void OnTick(IResourceGenerator resourceGenerator)
        {
            if (_progressBar != null && resourceGenerator != null)
            {
                _progressBar.value = resourceGenerator.GetTimerNormalized;
            }
        }

        private void UpdateUI()
        {
            if (_currentResourceGenerator == null) return;

            if (_resourceTypeImage != null && _currentResourceGenerator.ResourceType != null)
            {
                _resourceTypeImage.sprite = _currentResourceGenerator.ResourceType.ResourceSprite;
            }

            if (_resourceGatheringCountText != null)
            {
                _resourceGatheringCountText.text = _currentResourceGenerator.GetAmountGeneratedPerSecond.ToString("F1");
            }
        }
    }
}