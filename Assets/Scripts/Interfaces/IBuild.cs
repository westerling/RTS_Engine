using System.Collections;

public interface IBuild
{
    IEnumerator Build();

    void RpcAddBuilder(Building building, Unit unit);

    void SetNewTarget();

    void ResetBuilder();
}
