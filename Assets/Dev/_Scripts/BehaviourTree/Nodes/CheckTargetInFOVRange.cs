using UnityEngine;

namespace RPG.BehaviourTree
{
    public class CheckTargetInFOVRange : Node
    {
        private LayerMask _targetMask;
        private Transform _transform;
        private float _fovRange;

        public CheckTargetInFOVRange(
            LayerMask targetMask,
            Transform transform,
            float fovRange)
        {
            _targetMask = targetMask;
            _transform = transform;
            _fovRange = fovRange;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");

            // If there is no target, check if there is any in the FOV range
            if (target == null)
            {
                // Check if there is any target in FOV range, if its, save data to root
                Collider[] colliders = Physics.OverlapSphere(_transform.position, _fovRange, _targetMask);
                if (colliders.Length > 0)
                {
                    parent.parent.SetData("target", colliders[0].transform);

                    _state = NodeState.SUCCESS;
                    return _state;
                }

                // If there is no target in the FOV range, return failure
                _state = NodeState.FAILURE;
                return _state;
            }

            // If there was a target, check if it is out the FOV range to process suspicious
            bool isSuspicious = Vector3.Distance(_transform.position, target.position) > _fovRange;
            parent.SetData("isSuspicious", isSuspicious);

            _state = NodeState.SUCCESS;
            return _state;
        }
    }
}