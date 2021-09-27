using System.Collections.Generic;

namespace BehaviorTree
{
    public class Node
    {
        protected NodeState m_State;
        public NodeState State { get => m_State; }

        private Node m_Parent;
        private Dictionary<string, object> m_DataContext =
            new Dictionary<string, object>();

        protected List<Node> children = new List<Node>();

        public Node() { m_Parent = null; }

        public Node Parent
        { 
            get => m_Parent; 
        }

        public List<Node> Children
        {
            get => children; 
        }

        public bool HasChildren
        {
            get => children.Count > 0; 
        }

        public Node(List<Node> children) : this()
        {
            SetChildren(children);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        public void SetChildren(List<Node> children)
        {
            foreach (Node c in children)
                Attach(c);
        }

        public void Attach(Node child)
        {
            children.Add(child);
            child.m_Parent = this;
        }

        public void Detach(Node child)
        {
            children.Remove(child);
            child.m_Parent = null;
        }

        public object GetData(string key)
        {
            object value = null;
            if (m_DataContext.TryGetValue(key, out value))
                return value;

            Node node = m_Parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                    return value;
                node = node.m_Parent;
            }
            return null;
        }

        public bool ClearData(string key)
        {
            if (m_DataContext.ContainsKey(key))
            {
                m_DataContext.Remove(key);
                return true;
            }

            Node node = m_Parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node.m_Parent;
            }
            return false;
        }

        public void SetData(string key, object value)
        {
            m_DataContext[key] = value;
        }


    }
}