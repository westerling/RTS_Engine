using System.Collections;
using System;

public interface ICollector
{
    event Action<int> ResourceCollected;

    IEnumerator Collect();

    Resource CurrentResource { get; set; }

    int CarryingAmount { get; set; }

    bool CarryingAmountIsFull();

    void AddResource(Resource resource);

    void FindNewResource(Resource newResource);
}
