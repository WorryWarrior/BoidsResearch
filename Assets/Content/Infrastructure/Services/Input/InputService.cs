using System.Collections.Generic;
using Content.Infrastructure.Services.Logging;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Content.Infrastructure.Services.Input
{
    public class InputService : IInputService
    {
        private readonly PlayerControls _playerControls;
        private readonly ILoggingService _loggingService;
        
        public float MoveValue { get; private set; }
        public bool LookEnabledValue { get; private set; }

        public InputService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
            
            _playerControls = new PlayerControls();
            _playerControls.Enable();
            SubscribeControls();
        }
        
        private void SubscribeControls()
        {
            _playerControls.Player.Move.performed += OnMove;
            _playerControls.Player.Move.canceled += OnMove;
            _playerControls.Player.ToggleLookAround.performed += OnLookAroundToggled;
            _playerControls.Player.ToggleLookAround.canceled += OnLookAroundToggled;
        }
        
        private void OnMove(InputAction.CallbackContext context)
        {
            if (IsCursorOverUI())
            {
                MoveValue = 0f;
                return;
            }

            MoveValue = context.ReadValue<float>();
        }
        
        private void OnLookAroundToggled(InputAction.CallbackContext context)
        {
            LookEnabledValue = context.ReadValueAsButton();
        }

        private bool IsCursorOverUI()
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = Mouse.current.position.ReadValue()
            };
            
            List<RaycastResult> raycastResultsList = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResultsList);

            return raycastResultsList.Count > 0;
        }
    }
}