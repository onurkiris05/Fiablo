using RPG.Control;

namespace RPG.BehaviourTree
{
    public class CheckIsDead : Node
    {
        private ControllerBase _controller;

        public CheckIsDead(ControllerBase controller)
        {
            _controller = controller;
        }

        public override NodeState Evaluate()
        {
            _state = _controller.IsDead ? NodeState.SUCCESS : NodeState.FAILURE;
            return _state;
        }
    }
}