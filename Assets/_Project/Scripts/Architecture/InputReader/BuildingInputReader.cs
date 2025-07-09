using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Architecture.InputReader
{
    public interface IBuildingInputReader : IBaseInputReader
    {
        public Vector2 PointPosition { get; }
        public event Action Place;
        public event Action Remove;
    };
    
    public class BuildingInputReader : BaseInputReader, IBuildingInputReader
    {
        public Vector2 PointPosition => _pointPosition.ReadValue<Vector2>();
        public event Action Place;
        public event Action Remove;

        private BuilderDefenderActions.BuildingActions _actions;
        
        private readonly InputAction _pointPosition;
        private readonly InputAction _placeAction;
        private readonly InputAction _removeAction;
        
        public BuildingInputReader(BuilderDefenderActions inputActions): base()
        {
            if (inputActions == null)
                throw new ArgumentNullException($"BuildingInputReader: inputActions cannot be null.");
            
            _actions = inputActions.Building;
            
            _pointPosition = _actions.PointPosition;
            _placeAction = _actions.Place;
            _removeAction = _actions.Remove;
        }
        
        public override void Enable()
        {
            base.Enable();
            _actions.Enable();
            
            if (_placeAction != null)
                _placeAction.performed += OnPlace;
            if (_removeAction != null)
                _removeAction.performed += OnRemove;
        }

        public override void Disable()
        {
            base.Disable();
            _actions.Disable();
            
            if (_placeAction != null)
                _placeAction.performed -= OnPlace;
            if (_removeAction != null)
                _removeAction.performed -= OnRemove;
        }
        
        private void OnPlace(InputAction.CallbackContext obj)
        {
            if (ShouldIgnoreInput()) return;

            Place?.Invoke();
        }

        private void OnRemove(InputAction.CallbackContext obj)
        {
            if (ShouldIgnoreInput()) return;

            Remove?.Invoke();
        }
    }
}