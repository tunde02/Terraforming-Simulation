using System.Collections;
using System.Collections.Generic;
using System;

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
    public double Weight { get; set; }
    public long Consumption { get; set; }

    public Action(ActionType type)
    {
        Type = type;

        switch (Type)
        {
            case ActionType.Breed:
                Income = 7089471384;
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

        Weight = 1.0;
    }

    public List<(ResourceType resourceType, long amount)> PerformAction(Resource necessaryResource, Resource targetResource)
    {
        var variations = new List<(ResourceType resourceType, long amount)>();
        long actualIncome = (long)(Income * Weight);

        necessaryResource.Storage -= Consumption;
        targetResource.Storage += actualIncome;

        variations.Add((necessaryResource.resourceType, -Consumption));
        variations.Add((targetResource.resourceType, actualIncome));

        return variations;
    }

    public bool IsPerformable(Resource necessaryResource)
    {
        return necessaryResource.Storage - Consumption > 0;
    }
}
