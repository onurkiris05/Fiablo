using UnityEngine;

namespace RPG.Core
{
    public class AnimationHandler : MonoBehaviour
    {
        private Animator _animator;

        public void Init(Animator animator)
        {
            _animator = animator;
        }

        public void UpdateLocomotion(Vector3 currentVelocity)
        {
            var velocity = currentVelocity;
            var localVelocity = transform.InverseTransformDirection(velocity);
            var speed = localVelocity.z;
            _animator.SetFloat("forwardSpeed", speed);
        }

        public void SetTrigger(string triggerName)
        {
            _animator.SetTrigger(triggerName);
        }

        public void SetImmediate(string triggerName)
        {
            _animator.Play(triggerName, 0, 1f);
        }

        public void ResetTrigger(string triggerName)
        {
            _animator.ResetTrigger(triggerName);
        }
    }
}