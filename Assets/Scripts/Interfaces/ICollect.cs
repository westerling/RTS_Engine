using System.Collections;

public interface ICollect
{
    IEnumerator Collect();

    void FindNewResource(Resource newResource);
}
