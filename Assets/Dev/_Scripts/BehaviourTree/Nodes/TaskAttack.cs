using RPG.Control;
using RPG.Core;
using UnityEngine;

namespace RPG.BehaviourTree
{
    public class TaskAttack : Node
    {
        private ControllerBase _controller;

        public TaskAttack(ControllerBase controller)
        {
            _controller = controller;
        }

        public override NodeState Evaluate()
        {
            Transform t = (Transform)GetData("target");

            if (t.TryGetComponent(out HealthHandler target))
                _controller.ProcessAttack(target);

            _state = NodeState.RUNNING;
            return _state;
        }
    }
}