using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Architecture.InputReader
{
    public interface ICameraInputReader : IBaseInputReader
    {
        public event Action<Vector2> MoveCamera;
        public event Action<float> ScrollCamera;
    }

    public class CameraInputReader : BaseInputReader, ICameraInputReader
    {
        public event Action<Vector2> MoveCamera;
        public event Action<float> ScrollCamera;

        private BuilderDefenderActions.CameraActions _actions;

        private readonly InputAction  _moveCameraAction;
        private readonly InputAction _scrollCameraAction;
        private readonly InputAction _scrollCameraUpAction;
        private readonly InputAction _scrollCameraDownAction;
        
        public CameraInputReader(BuilderDefenderActions inputActions) :base()
        {
            if (inputActions == null)
                throw new ArgumentNullException($"CameraInputReader: inputActions cannot be null.");
            
            _actions = inputActions.Camera;
            
            _moveCameraAction = _actions.MoveCamera;
            _scrollCameraAction = _actions.Scroll;
            _scrollCameraUpAction = _actions.ScrollUp;
            _scrollCameraDownAction = _actions.ScrollDown;
        }

        public override void Enable()
        {
            base.Enable();
            _actions.Enable();
            
            _moveCameraAction.performed += OnMoveCamera;
            _moveCameraAction.canceled += OnMoveCamera;
            
            _scrollCameraAction.performed += OnMouseScroll;
            _scrollCameraUpAction.performed += OnScrollUp;
            _scrollCameraDownAction.performed += OnScrollDown;
        }

        

        public override void Disable()
        {
            base.Disable();
            _actions.Disable();
            
            _moveCameraAction.performed -= OnMoveCamera;
            _moveCameraAction.canceled -= OnMoveCamera;
            
            _scrollCameraAction.performed -= OnMouseScroll; 
            _scrollCameraUpAction.performed -= OnScrollUp;
            _scrollCameraDownAction.performed -= OnScrollDown;
        }
        
        private void OnMouseScroll(InputAction.CallbackContext obj)
        {
            if (ShouldIgnoreInput()) return;
            
            OnScroll(obj.ReadValue<float>());
        }
        
        private void OnScrollDown(InputAction.CallbackContext obj)
        {
            if (ShouldIgnoreInput()) return;
            
            if (Mathf.Approximately(obj.ReadValue<float>(), 1))
            {
                OnScroll(2);
            }
        }

        private void OnScrollUp(InputAction.CallbackContext obj)
        {
            if (ShouldIgnoreInput()) return;

            if (Mathf.Approximately(obj.ReadValue<float>(), 1))
            {
                OnScroll(-2);
            }
        }
        private void OnScroll(float value)
        {
            ScrollCamera?.Invoke(value);
        }
        
        private void OnMoveCamera(InputAction.CallbackContext obj)
        {
            if (ShouldIgnoreInput()) return;
            MoveCamera?.Invoke(obj.ReadValue<Vector2>());
        }
    }
}