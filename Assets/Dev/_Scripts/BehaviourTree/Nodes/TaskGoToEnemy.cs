using UnityEngine;

namespace RPG.BehaviourTree
{
    public class TaskGoToEnemy : Node
    {
        private PlayerController _controller;
        private Transform _transform;

        public TaskGoToEnemy(PlayerController controller, Transform transform)
        {
            _controller = controller;
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");

            if (Vector3.Distance(_transform.position, target.position) > 0.2f)
            {
                _controller.ProcessMove(target.position);
            }

            _state = NodeState.RUNNING;
            return _state;
        }
    }
}