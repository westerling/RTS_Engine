using System.Collections.Generic;
using System.Linq;

namespace BehaviorTree
{
    public class Sequence : Node
    {
        private bool m_IsRandom;

        public Sequence() : base() { m_IsRandom = false; }
        public Sequence(bool isRandom) : base() { m_IsRandom = isRandom; }
        public Sequence(List<Node> children, bool isRandom = false) : base(children)
        {
            m_IsRandom = isRandom;
        }

        public static List<T> Shuffle<T>(List<T> list)
        {
            System.Random r = new System.Random();
            return list.OrderBy(x => r.Next()).ToList();
        }

        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;
            if (m_IsRandom)
                children = Shuffle(children);

            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        m_State = NodeState.FAILURE;
                        return m_State;
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
            m_State = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return m_State;
        }
    }
}