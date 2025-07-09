using System;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Architecture.InputReader
{
    public interface IUIInputReader : IBaseInputReader
    {
        public event Action FirstHotbar;
        public event Action SecondHotbar;
        public event Action ThirdHotbar;
        public event Action FourthHotbar;
        public event Action FifthHotbar;

        public event Action Pause;
        public event Action Cancel;
    }
    
    public class UIInputReader : BaseInputReader, IUIInputReader
    {
        public event Action FirstHotbar;
        public event Action SecondHotbar;
        public event Action ThirdHotbar;
        public event Action FourthHotbar;
        public event Action FifthHotbar;
        public event Action Pause;
        public event Action Cancel;

        private BuilderDefenderActions.UIActions _actions;
        
        private readonly InputAction _firstHotbar;
        private readonly InputAction _secondHotbar;
        private readonly InputAction _thirdHotbar;
        private readonly InputAction _fourthHotbar;
        private readonly InputAction _fifthHotbar;
        private readonly InputAction _pause;
        private readonly InputAction _cancel;
        
        public UIInputReader(BuilderDefenderActions inputActions) : base()
        {
            if (inputActions == null)
                throw new ArgumentNullException($"UIInputReader: inputActions cannot be null.");
            
            _actions = inputActions.UI;
            
            _firstHotbar = _actions.FifthHotbar;
            _secondHotbar = _actions.SecondHotbar;
            _thirdHotbar = _actions.ThirdHotbar;
            _fourthHotbar = _actions.FourthHotbar;
            _fifthHotbar = _actions.FifthHotbar;
            _pause = _actions.Pause;
            _cancel = _actions.Cancel;
        }

        public override void Enable()
        {
            base.Enable();
            _actions.Enable();

            if (_firstHotbar != null) 
                _firstHotbar.performed += OnFirstHotbarPerformed;
            if (_secondHotbar != null)
                _secondHotbar.performed += OnSecondHotbarPerformed;
            if (_thirdHotbar != null) 
                _thirdHotbar.performed += OnThirdHotbarPerformed;
            if (_fourthHotbar != null) 
                _fourthHotbar.performed += OnFourthHotbarPerformed;
            if (_fifthHotbar != null) 
                _fifthHotbar.performed += OnFifthHotbarPerformed;
            if (_pause != null) 
                _pause.performed += OnPausePerformed;
            if (_cancel != null) 
                _cancel.performed += OnCancel;
        }
        
        public override void Disable()
        {
            base.Disable();
            _actions.Disable();
            
            if (_firstHotbar != null) 
                _firstHotbar.performed -= OnFirstHotbarPerformed;
            if (_secondHotbar != null)
                _secondHotbar.performed -= OnSecondHotbarPerformed;
            if (_thirdHotbar != null) 
                _thirdHotbar.performed -= OnThirdHotbarPerformed;
            if (_fourthHotbar != null) 
                _fourthHotbar.performed -= OnFourthHotbarPerformed;
            if (_fifthHotbar != null) 
                _fifthHotbar.performed -= OnFifthHotbarPerformed;
            if (_pause != null) 
                _pause.performed -= OnPausePerformed;
            if (_cancel != null) 
                _cancel.performed -= OnCancel;
        }
        private void OnCancel(InputAction.CallbackContext obj)
        {
            if (ShouldIgnoreInput()) return;
            Cancel?.Invoke();
        }

        private void OnPausePerformed(InputAction.CallbackContext obj)
        {
            if (ShouldIgnoreInput()) return;
            Pause?.Invoke();
        }

        private void OnFifthHotbarPerformed(InputAction.CallbackContext obj)
        {
            if (ShouldIgnoreInput()) return;
            FifthHotbar?.Invoke();
        }

        private void OnFourthHotbarPerformed(InputAction.CallbackContext obj)
        {
            if (ShouldIgnoreInput()) return;
            FourthHotbar?.Invoke();
        }

        private void OnThirdHotbarPerformed(InputAction.CallbackContext obj)
        {
            if (ShouldIgnoreInput()) return;
            ThirdHotbar?.Invoke();
        }

        private void OnSecondHotbarPerformed(InputAction.CallbackContext obj)
        {
            if (ShouldIgnoreInput()) return;
            SecondHotbar?.Invoke();
        }

        private void OnFirstHotbarPerformed(InputAction.CallbackContext obj)
        {
            if (ShouldIgnoreInput()) return;
            FirstHotbar?.Invoke();
        }
    }
}