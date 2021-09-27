using System.Collections.Generic;

namespace BehaviorTree
{
    public class Inverter : Node
    {
        public Inverter() : base() { }
        public Inverter(List<Node> m_Children) : base(m_Children) { }

        public override NodeState Evaluate()
        {
            if (!HasChildren) return NodeState.FAILURE;
            switch (children[0].Evaluate())
            {
                case NodeState.FAILURE:
                    m_State = NodeState.SUCCESS;
                    return m_State;
                case NodeState.SUCCESS:
                    m_State = NodeState.FAILURE;
                    return m_State;
                case NodeState.RUNNING:
                    m_State = NodeState.RUNNING;
                    return m_State;
                default:
                    m_State = NodeState.FAILURE;
                    return m_State;
            }
        }
    }
}