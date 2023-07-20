
namespace RPG.BehaviourTree.Decorators
{
    public class Inverter : Node
    {
        protected Node _node;

        public Inverter(Node node)
        {
            _node = node;
        }

        public override NodeState Evaluate()
        {
            switch (_node.Evaluate())
            {
                case NodeState.RUNNING:
                    _state = NodeState.RUNNING;
                    break;
                case NodeState.SUCCESS:
                    _state = NodeState.FAILURE;
                    break;
                case NodeState.FAILURE:
                    _state = NodeState.SUCCESS;
                    break;
            }

            return _state;
        }
    }
}