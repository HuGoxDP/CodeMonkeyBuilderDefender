using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Architecture.InputReader
{
    public interface IInputManager
    {
        public IBuildingInputReader BuildingInputReader { get; }
        public ICameraInputReader CameraInputReader { get; }
        public IUIInputReader UIInputReader { get; }
    }

    public class InputManager : MonoBehaviour, IInputManager
    {
        public IBuildingInputReader BuildingInputReader { get; private set; }
        public ICameraInputReader CameraInputReader { get; private set; }
        public IUIInputReader UIInputReader { get; private set;}

        private BuilderDefenderActions _actions;
        private void Awake()
        {
            _actions = new BuilderDefenderActions();
            BuildingInputReader = new BuildingInputReader(_actions);
            CameraInputReader = new CameraInputReader(_actions);
            UIInputReader = new UIInputReader(_actions);

            EnableBuildingInputReader();
            EnableCameraInputReader();
            EnableUIInputReader();
        }

        private void OnDestroy()
        {
            DisableBuildingInputReader();
            DisableCameraInputReader();
            DisableUIInputReader();
            
            BuildingInputReader.Dispose();
            CameraInputReader.Dispose();
            UIInputReader.Dispose();
        }
        
        public void EnableBuildingInputReader()
        {
            BuildingInputReader.Enable();
        }

        public void DisableBuildingInputReader()
        {
            BuildingInputReader.Disable();
        }

        public void EnableCameraInputReader()
        {
            CameraInputReader.Enable();
        }

        public void DisableCameraInputReader()
        {
            CameraInputReader.Disable();
        }

        public void EnableUIInputReader()
        {
            UIInputReader.Enable();
        }

        public void DisableUIInputReader()
        {
            UIInputReader.Disable();
        }
    }

    public interface IBaseInputReader : IDisposable
    {
        public void Enable();
        public void Disable();
        public bool IsEnable { get; }
    }

    public abstract class BaseInputReader : IBaseInputReader
    {
        public bool IsEnable { get; private set; } = true;
        private bool _justRegainedFocus;

        protected BaseInputReader()
        {
            Application.focusChanged += OnFocusChanged;
        }

        public virtual void Dispose()
        {
            Disable();
            Application.focusChanged -= OnFocusChanged;
        }

        public virtual void Enable()
        {
            IsEnable = true;
        }

        public virtual void Disable()
        {
            IsEnable = false;
        }

        protected virtual void OnFocusChanged(bool hasFocus)
        {
            if (hasFocus)
            {
                _justRegainedFocus = true;
            }
        }

        protected virtual bool ShouldIgnoreInput()
        {
            if (_justRegainedFocus || !IsEnable)
            {
                _justRegainedFocus = false;
                return true;
            }

            return false;
        }
    }
}