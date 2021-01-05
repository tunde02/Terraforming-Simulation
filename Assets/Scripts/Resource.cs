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
    public ResourceType resourceType;
    public string name;
    public long Storage { get; set; }

    public Resource(ResourceType resourceType, string name, long storage, long income)
    {
        this.resourceType = resourceType;
        this.name = name;
        Storage = storage;
    }
}
