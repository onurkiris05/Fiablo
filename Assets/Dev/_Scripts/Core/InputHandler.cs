using UnityEngine;
using Lean.Touch;

namespace RPG.Core
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private float fingerMoveTolerance = 30f;
        
        private PlayerController _player;
        private LeanFinger _finger;

        public void Init(PlayerController player)
        {
            _player = player;
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

                _player.ProcessInputOnFingerDown(_finger);
            }
        }

        private void HandleFingerUpdate(LeanFinger movedFinger)
        {
            if (movedFinger == _finger && IsMoved(movedFinger))
            {
                _player.ProcessInputOnFingerMove(_finger);
            }
        }

        private void HandleFingerUp(LeanFinger lostFinger)
        {
            if (lostFinger == _finger)
            {
                _finger = null;
            }
        }
        
        private bool IsMoved(LeanFinger movedFinger)
        {
            var dist=(movedFinger.LastScreenPosition - movedFinger.StartScreenPosition).sqrMagnitude;
            return dist > fingerMoveTolerance.Sqr();
        }
    }
}