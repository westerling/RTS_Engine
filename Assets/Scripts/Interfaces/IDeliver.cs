using System.Collections;

public interface IDeliver
{
    IEnumerator Deliver();

    void ResetCollector();
}
