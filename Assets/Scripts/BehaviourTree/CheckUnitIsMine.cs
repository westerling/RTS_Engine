using BehaviorTree;

public class CheckUnitIsMine : Node
{
    private bool m_UnitIsMine;

    public CheckUnitIsMine() : base()
    {
        m_UnitIsMine = false;
    }

    public override NodeState Evaluate()
    {
        m_State = m_UnitIsMine ? NodeState.SUCCESS : NodeState.FAILURE;
        return m_State;
    }
}