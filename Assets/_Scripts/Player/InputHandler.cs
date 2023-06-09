using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

namespace RPG.Player
{
    public class InputHandler : MonoBehaviour
    {
        private PlayerController  _playerController;
        private Finger _finger;
    
        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
        }
    
        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
    
            ETouch.Touch.onFingerDown += HandleFingerDown;
            ETouch.Touch.onFingerUp += HandleFingerUp;
            ETouch.Touch.onFingerMove += HandleFingerMove;
        }
    
        private void OnDisable()
        {
            ETouch.Touch.onFingerDown -= HandleFingerDown;
            ETouch.Touch.onFingerUp -= HandleFingerUp;
            ETouch.Touch.onFingerMove -= HandleFingerMove;
    
            EnhancedTouchSupport.Disable();
        }
    
        private void HandleFingerDown(Finger touchedFinger)
        {
            if (_finger == null)
            {
                _finger = touchedFinger;
    
                _playerController.ProcessInput(_finger.currentTouch.screenPosition);
            }
        }
    
        private void HandleFingerUp(Finger lostFinger)
        {
            if (lostFinger == _finger)
            {
                _finger = null;
            }
        }
    
        private void HandleFingerMove(Finger movedFinger)
        {
            if (movedFinger == _finger)
            {
                _playerController.ProcessInput(_finger.currentTouch.screenPosition);
            }
        }
    }
}