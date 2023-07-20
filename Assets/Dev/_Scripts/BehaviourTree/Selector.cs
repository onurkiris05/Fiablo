using System.Collections.Generic;

namespace RPG.BehaviourTree
{
    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            foreach (var child in _children)
            {
                switch (child.Evaluate())
                {
                    case NodeState.RUNNING:
                        _state = NodeState.RUNNING;
                        return _state;
                    case NodeState.SUCCESS:
                        _state = NodeState.SUCCESS;
                        return _state;
                    case NodeState.FAILURE:
                        break;
                }
            }

            _state = NodeState.FAILURE;
            return _state;
        }
    }
}