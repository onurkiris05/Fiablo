using UnityEngine;

namespace RPG.BehaviourTree
{
    public class TaskGoToPlayer : Node
    {
        private EnemyController _controller;
        private Transform _transform;
        private float _suspiciousTime;

        private float _timer;
        private bool _isSuspicious;

        public TaskGoToPlayer(
            EnemyController controller,
            Transform transform,
            float suspiciousTime)
        {
            _controller = controller;
            _transform = transform;
            _suspiciousTime = suspiciousTime;
        }

        public override NodeState Evaluate()
        {
            if (GetData("isSuspicious") is bool isSuspicious)
                _isSuspicious = isSuspicious;

            if (_isSuspicious)
            {
                _timer += Time.deltaTime;
                if (_timer >= _suspiciousTime)
                {
                    parent.SetData("isSuspicious", false);
                    ClearData("target");
                }
            }
            else
            {
                Transform target = (Transform)GetData("target");

                if (Vector3.Distance(_transform.position, target.position) > 0.2f)
                {
                    _controller.ProcessMove(target.position);
                    _timer = 0;
                }
            }

            _state = NodeState.RUNNING;
            return _state;
        }
    }
}