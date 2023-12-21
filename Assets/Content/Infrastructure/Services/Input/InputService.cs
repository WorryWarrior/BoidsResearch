using Content.Infrastructure.Services.Logging;
using UnityEngine.InputSystem;

namespace Content.Infrastructure.Services.Input
{
    public class InputService : IInputService
    {
        private readonly PlayerControls _playerControls;
        private readonly ILoggingService _loggingService;
        
        public float MoveValue { get; private set; }
        
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
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            MoveValue = context.ReadValue<float>();
            //_loggingService.LogMessage($"Move Value changed to: {MoveValue}", this);
        }
    }
}