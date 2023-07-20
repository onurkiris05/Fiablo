using RPG.Control;
using UnityEngine;

namespace RPG.BehaviourTree
{
    public class TaskPatrol : Node
    {
        private EnemyController _controller;
        private PatrolPath _path;
        private float _dwellTime;

        private float _timer;
        private bool _waiting;
        private int _currentWaypointIndex;

        public TaskPatrol(EnemyController controller, PatrolPath path, float dwellTime)
        {
            _controller = controller;
            _path = path;
            _dwellTime = dwellTime;
        }

        public override NodeState Evaluate()
        {
            if (_waiting)
            {
                _timer += Time.deltaTime;
                if (_timer >= _dwellTime)
                    _waiting = false;
            }
            else
            {
                var wp = _path.GetWaypoint(_currentWaypointIndex);
                if (Vector3.Distance(_controller.transform.position, wp) < 0.3f)
                {
                    _waiting = true;
                    _currentWaypointIndex = _path.GetIndex(_currentWaypointIndex + 1);
                }
                else
                {
                    _controller.ProcessMove(wp);
                    _timer = 0;
                }
            }

            _state = NodeState.RUNNING;
            return _state;
        }
    }
}