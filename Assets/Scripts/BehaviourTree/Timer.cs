using UnityEngine;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class Timer : Node
    {
        private float m_Delay;
        private float m_Time;

        public delegate void TickEnded();
        public event TickEnded onTickEnded;

        public Timer(float delay, TickEnded onTickEnded = null) : base()
        {
            m_Delay = delay;
            m_Time = m_Delay;
            this.onTickEnded = onTickEnded;
        }
        public Timer(float delay, List<Node> children, TickEnded onTickEnded = null)
            : base(children)
        {
            m_Delay = delay;
            m_Time = m_Delay;
            this.onTickEnded = onTickEnded;
        }

        public override NodeState Evaluate()
        {
            if (!HasChildren) return NodeState.FAILURE;
            if (m_Time <= 0)
            {
                m_Time = m_Delay;
                m_State = children[0].Evaluate();
                if (onTickEnded != null)
                    onTickEnded();
                m_State = NodeState.SUCCESS;
            }
            else
            {
                m_Time -= Time.deltaTime;
                m_State = NodeState.RUNNING;
            }
            return m_State;
        }
    }
}