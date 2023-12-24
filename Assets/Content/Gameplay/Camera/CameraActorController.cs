using Cinemachine;
using Content.Infrastructure.Services.Input;
using UnityEngine;
using Zenject;

namespace Content.Gameplay.Camera
{
    public class CameraActorController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 25f;
        
        [Header("Cinemachine Data")]
        [SerializeField] private CinemachineVirtualCameraBase cinemachineCamera = null; 
        [SerializeField] private CinemachineInputProvider cinemachineInputProvider = null;
        
        private IInputService _inputService;
        
        [Inject]
        private void Construct(
            IInputService inputService)
        {
            _inputService = inputService;
        }
        
        private void Update()
        {
            if (_inputService.MoveValue > 0.001f)
            {
                transform.Translate(movementSpeed * Time.deltaTime * Vector3.forward);
            }
            
            cinemachineInputProvider.enabled = _inputService.LookEnabledValue;
        
            transform.rotation = Quaternion.LookRotation(transform.position - cinemachineCamera.transform.position);
        }
    }
}
