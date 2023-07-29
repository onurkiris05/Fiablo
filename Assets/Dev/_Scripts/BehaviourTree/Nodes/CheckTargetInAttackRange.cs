using RPG.Control;
using UnityEngine;

namespace RPG.BehaviourTree
{
    public class CheckTargetInAttackRange : Node
    {
        private LayerMask _targetMask;
        private Transform _transform;
        private ControllerBase _character;

        public CheckTargetInAttackRange(
            LayerMask targetMask,
            Transform transform,
            ControllerBase character)
        {
            _targetMask = targetMask;
            _transform = transform;
            _character = character;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");

            // If there is target, check if there is any in the attack range
            if (target != null)
            {
                // Check if there is any target in attack range
                Collider[] colliders = Physics.OverlapSphere(_transform.position, _character.WeaponRange, _targetMask);
                if (colliders.Length > 0)
                {
                    _state = NodeState.SUCCESS;
                    return _state;
                }
            }

            _state = NodeState.FAILURE;
            return _state;
        }
    }
}