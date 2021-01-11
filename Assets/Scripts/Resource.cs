using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum ResourceType
{
    Population,
    Food,
    DNA,
    Power
}

public class Resource
{
    public readonly ResourceType resourceType;
    public long Storage { get; set; }

    public Resource(ResourceType resourceType, long storage)
    {
        this.resourceType = resourceType;
        Storage = storage;
    }
}
