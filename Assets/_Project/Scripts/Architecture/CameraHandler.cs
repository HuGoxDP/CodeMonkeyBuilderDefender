using _Project.Scripts.Architecture.InputReader;
using _Project.Scripts.Architecture.UnityServiceLocator;
using Unity.Cinemachine;
using UnityEngine;

namespace _Project.Scripts.Architecture
{
    public class CameraHandler: MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _cinemachineCamera;
        [SerializeField] private float _minZoom;
        [SerializeField] private float _maxZoom;
        [SerializeField] private float _zoomSpeed = 5f;
        
        [SerializeField] private float _movingSpeed;
        
        private float _orthographicSize;
        private float _targetOrthographicSize;
        
        private ICameraInputReader _cameraInputReader;
        private Vector2 _movementDirection;
        
        private void Start()
        {
            ServiceLocator.Instance.GetService(out IInputManager inputManager);
            _cameraInputReader = inputManager.CameraInputReader;
            
            if (_cinemachineCamera != null)
            {
                _orthographicSize = _cinemachineCamera.Lens.OrthographicSize;
                _targetOrthographicSize = _orthographicSize;
            }
        
            _cameraInputReader.MoveCamera += InputMovementDirection;
            _cameraInputReader.ScrollCamera += OnScroll;
        }

        private void OnScroll(float value)
        {
            if (_cinemachineCamera == null) return;
            _targetOrthographicSize += value;
        }
        
        private void OnDestroy()
        {
            _cameraInputReader.MoveCamera -= InputMovementDirection;
            _cameraInputReader.ScrollCamera -= OnScroll;
        }

        private void Update()
        {
            HandleMovement();
            HandleZoom();
        }

        private void HandleZoom()
        {
            _targetOrthographicSize = Mathf.Clamp(_targetOrthographicSize, _minZoom, _maxZoom);
            _orthographicSize = Mathf.Lerp(_orthographicSize, _targetOrthographicSize, _zoomSpeed * Time.deltaTime);
            _cinemachineCamera.Lens.OrthographicSize = _orthographicSize;
        }

        private void HandleMovement()
        {
            var nextPosition = _movementDirection * (_movingSpeed * Time.deltaTime);
            transform.position += new Vector3(nextPosition.x, nextPosition.y);
        }
        
        private void InputMovementDirection(Vector2 movementDirection)
        {
            _movementDirection =  movementDirection;
        }
    }
}