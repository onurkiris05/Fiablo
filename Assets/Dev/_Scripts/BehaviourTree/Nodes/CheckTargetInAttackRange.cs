using UnityEngine;

namespace RPG.BehaviourTree
{
    public class CheckTargetInAttackRange : Node
    {
        private LayerMask _targetMask;
        private Transform _transform;
        private float _attackRange;

        public CheckTargetInAttackRange(
            LayerMask targetMask,
            Transform transform,
            float attackRange)
        {
            _targetMask = targetMask;
            _transform = transform;
            _attackRange = attackRange;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");

            // If there is target, check if there is any in the attack range
            if (target != null)
            {
                // Check if there is any target in attack range
                Collider[] colliders = Physics.OverlapSphere(_transform.position, _attackRange, _targetMask);
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