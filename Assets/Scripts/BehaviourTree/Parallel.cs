using System.Collections.Generic;

namespace BehaviorTree
{
    public class Parallel : Node
    {
        public Parallel() : base() { }
        public Parallel(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;
            int nFailedChildren = 0;
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        nFailedChildren++;
                        continue;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        m_State = NodeState.SUCCESS;
                        return m_State;
                }
            }
            if (nFailedChildren == children.Count)
                m_State = NodeState.FAILURE;
            else
                m_State = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return m_State;
        }
    }
}