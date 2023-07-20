namespace RPG.BehaviourTree
{
    public class CheckIsSafe : Node
    {
        private PlayerController _controller;

        public CheckIsSafe(PlayerController controller)
        {
            _controller = controller;
        }

        public override NodeState Evaluate()
        {
            if (_controller.Target != null)
            {
                parent.SetData("target", _controller.Target);

                _state = NodeState.FAILURE;
                return _state;
            }

            ClearData("target");

            _state = NodeState.SUCCESS;
            return _state;
        }
    }
}