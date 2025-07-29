using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Architecture.InputReader
{
    public interface IBuildingInputReader : IBaseInputReader, IPointerPositionInputReader
    {
        public event Action Place;
        public event Action Remove;
    };

    public interface IPointerPositionInputReader
    {
        public Vector2 PointPosition { get; }
    }

    public class BuildingInputReader : BaseInputReader, IBuildingInputReader
    {
        private readonly InputAction _placeAction;

        private readonly InputAction _pointPosition;
        private readonly InputAction _removeAction;

        private BuilderDefenderActions.BuildingActions _actions;

        public BuildingInputReader(BuilderDefenderActions inputActions) : base()
        {
            if (inputActions == null)
                throw new ArgumentNullException($"BuildingInputReader: inputActions cannot be null.");

            _actions = inputActions.Building;

            _pointPosition = _actions.PointPosition;
            _placeAction = _actions.Place;
            _removeAction = _actions.Remove;
        }

        public Vector2 PointPosition => _pointPosition.ReadValue<Vector2>();
        public event Action Place;
        public event Action Remove;

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

        private async void OnPlace(InputAction.CallbackContext obj)
        {
            if (ShouldIgnoreInput()) return;
            bool isOverUI = await CheckPointerOverUI();

            if (!isOverUI)
            {
                Place?.Invoke();
            }
        }

        private async void OnRemove(InputAction.CallbackContext obj)
        {
            if (ShouldIgnoreInput()) return;
            bool isOverUI = await CheckPointerOverUI();

            if (!isOverUI)
            {
                Remove?.Invoke();
            }
        }

        private async UniTask<bool> CheckPointerOverUI()
        {
            await UniTask.NextFrame();
            return IsPointerOverUI(PointPosition);
        }

        private bool IsPointerOverUI(Vector2 position)
        {
            if (EventSystem.current == null) return false;

            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = position
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            return results.Count > 0;
        }
    }
}