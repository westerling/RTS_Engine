using System.Collections;

public interface IBuilder
{
    IEnumerator Build();

    CreateEntity[] Buildings { get; set; }

    void RpcAddBuilder(Building building, Unit unit);

    void FindNewBuilding();

    void AddSwitchPanelsAction();
}
