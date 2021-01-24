using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public enum ResourceType { POPULATION, FOOD, DNA, POWER }

public class Resource
{
    public delegate void StorageSetHandler(Resource resource);
    public delegate void StorageChangeHandler(Resource resource, long prev);

    public static event StorageSetHandler OnStorageSet;
    public static event StorageChangeHandler OnProduced;
    public static event StorageChangeHandler OnConsumed;


    public ResourceType Type { get; }
    private long storage;
    public long Storage
    {
        get
        {
            return storage;
        }
        set
        {
            storage = value;

            OnStorageSet(this);
        }
    }


    public Resource(ResourceType resourceType, long storage)
    {
        Type = resourceType;
        Storage = storage;
    }

    public void Produce(long amount)
    {
        long prev = Storage;

        Storage += amount;

        OnProduced(this, prev);
    }

    public void Consume(long amount)
    {
        long prev = Storage;

        Storage -= amount;

        OnConsumed(this, prev);
    }
}
