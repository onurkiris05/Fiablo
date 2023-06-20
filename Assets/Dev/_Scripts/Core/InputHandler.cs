using UnityEngine;
using Lean.Touch;
using RPG.Control;

namespace RPG.Core
{
    public class InputHandler : MonoBehaviour
    {
        private PlayerController _playerController;
        private LeanFinger _finger;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            LeanTouch.OnFingerDown += HandleFingerDown;
            LeanTouch.OnFingerUpdate += HandleFingerUpdate;
            LeanTouch.OnFingerUp += HandleFingerUp;
        }

        private void OnDisable()
        {
            LeanTouch.OnFingerDown -= HandleFingerDown;
            LeanTouch.OnFingerUpdate -= HandleFingerUpdate;
            LeanTouch.OnFingerUp -= HandleFingerUp;
        }

        private void HandleFingerDown(LeanFinger touchedFinger)
        {
            if (_finger == null)
            {
                _finger = touchedFinger;

                _playerController.ProcessInput(touchedFinger.ScreenPosition);
            }
        }

        private void HandleFingerUpdate(LeanFinger movedFinger)
        {
            if (movedFinger == _finger)
            {
                _playerController.ProcessInput(movedFinger.ScreenPosition);
            }
        }
        
        private void HandleFingerUp(LeanFinger lostFinger)
        {
            if (lostFinger == _finger)
            {
                _finger = null;
            }
        }
    }
}