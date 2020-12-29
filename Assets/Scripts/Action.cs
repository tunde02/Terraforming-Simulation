using System.Collections;
using System.Collections.Generic;


public enum ActionType
{
    Breed,
    Hunt,
    Evolve,
    Train
}

public class Action
{
    public ActionType Type { get; }
    public long Income { get; set; }
    public float Weight { get; set; }
    public long Consumption { get; set; }

    public Action(ActionType type)
    {
        Type = type;

        switch (Type)
        {
            case ActionType.Breed:
                Income = 100;
                Consumption = 0;
                break;
            case ActionType.Hunt:
                Income = 10;
                Consumption = 30;
                break;
            case ActionType.Evolve:
                Income = 2;
                Consumption = 50;
                break;
            case ActionType.Train:
                Income = 10;
                Consumption = 50;
                break;
        }

        Weight = 1.0f;
    }

    public List<(ResourceType resourceType, long amount)> PerformAction(Resource necessaryResource, Resource targetResource)
    {
        var variations = new List<(ResourceType resourceType, long amount)>();

        necessaryResource.Storage -= Consumption;
        targetResource.Storage += (long)(Income * Weight);

        variations.Add((necessaryResource.resourceType, -Consumption));
        variations.Add((targetResource.resourceType, (long)(Income * Weight)));

        return variations;
    }
}
