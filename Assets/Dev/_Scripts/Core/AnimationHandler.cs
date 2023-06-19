using UnityEngine;

namespace RPG.Core
{
    public class AnimationHandler : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void UpdateAnimation(Vector3 currentVelocity)
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
    }
}