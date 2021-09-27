using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node m_Root = null;

        public Node Root => m_Root;
        protected abstract Node SetupTree();

        protected void Start()
        {
            m_Root = SetupTree();
        }

        private void Update()
        {
            if (m_Root != null)
                m_Root.Evaluate();
        }        
    }
}
