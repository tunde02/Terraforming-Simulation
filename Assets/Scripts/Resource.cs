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
    private readonly string[] units = { "", "a", "b", "c", "d", "e", "f" };

    public ResourceType resourceType;
    public string name;
    public long Storage { get; set; }

    public Resource(ResourceType resourceType, string name, long storage, long income)
    {
        this.resourceType = resourceType;
        this.name = name;
        Storage = storage;
    }

    public string GetStorageOverview()
    {
        int storageUnit = 0;
        double storageCompare = 1000;

        while (Storage >= storageCompare)
        {
            storageCompare *= 1000;
            storageUnit++;
        }

        return $"{Math.Floor(Storage / (storageCompare / 1000) * 10) * 0.1d}{units[storageUnit]}";
    }
}
