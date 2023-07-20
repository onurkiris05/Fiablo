using System.Collections.Generic;

namespace RPG.BehaviourTree
{
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            bool anyChildRunning = false;
            foreach (var child in _children)
            {
                switch (child.Evaluate())
                {
                    case NodeState.RUNNING:
                        anyChildRunning = true;
                        break;
                    case NodeState.SUCCESS:
                        break;
                    case NodeState.FAILURE:
                        _state = NodeState.FAILURE;
                        return _state;
                }
            }

            _state = anyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return _state;
        }
    }
}