using Mirror;

public class TargeterVillager : TargeterTest
{

    private Resource m_Resource;

    public Resource Resource 
    {
        get => m_Resource; 
        set => m_Resource = value;
    }

    public override void FindNewTarget(Task task)
    {
        var player = NetworkClient.connection.identity.GetComponent<RtsPlayer>();

        switch (task)
        {
            case Task.Build:
                Target = TargetFinder.FindNewBuilding(player, transform);
                break;
            case Task.Attack:
                Target = TargetFinder.FindNewEnemyUnit(transform, 10f);
                break;
            case Task.Collect:
                Target = TargetFinder.FindNewResource(transform, Resource);
                break;
            case Task.Deliver:
                Target = TargetFinder.FindNewBuilding(player, transform);
                break;
        }
    }

    public override bool PossibleTask(Task task)
    {
        switch (task)
        {
            case Task.Idle:
            case Task.Move:
            case Task.Build:
            case Task.Attack:
            case Task.Deliver:
            case Task.Collect:
            case Task.Garrison:
                return true;
        }
        return false;
    }

    public override void UpdateStats()
    {
        throw new System.NotImplementedException();
    }

    private void FindNewDeliveryPoint()
    {
        throw new System.NotImplementedException();
    }

    private void FindNewResource()
    {
        throw new System.NotImplementedException();
    }
}
