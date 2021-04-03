using System.Collections;
using System.Collections.Generic;
using System;


public enum ActionType { BREED, HUNT, EVOLVE, TRAIN }

public class Action
{
    public ActionType Type { get; }
    public long Income { get; set; }
    public long ActualIncome
    {
        get
        {
            long actualIncome = Income;
            foreach (double weight in Weights)
                actualIncome = (long)(actualIncome * weight);
            return actualIncome;
        }
    }
    public List<double> Weights { get; private set; }
    public long Consumption { get; set; }

    public Action(ActionType type)
    {
        Type = type;

        switch (Type)
        {
            case ActionType.BREED:
                Income = 100;
                Consumption = 0;
                break;
            case ActionType.HUNT:
                Income = 10;
                Consumption = 15;
                break;
            case ActionType.EVOLVE:
                Income = 2;
                Consumption = 25;
                break;
            case ActionType.TRAIN:
                Income = 10;
                Consumption = 25;
                break;
        }

        Weights = new List<double> {
            1.0
        };
    }

    public void PerformAction(List<Resource> resources, double blockWeight = 1.0f)
    {
        Resource consumedResource = null;
        Resource producedResource = null;
        long actualIncome = Income;
        foreach (double weight in Weights)
            actualIncome = (long)(actualIncome * weight);
        actualIncome = (long)(actualIncome * blockWeight);

        switch (Type)
        {
            case ActionType.BREED:
                consumedResource = resources[0];
                producedResource = resources[(int)Type];
                break;
            case ActionType.HUNT:
                consumedResource = resources[0];
                producedResource = resources[(int)Type];
                break;
            case ActionType.EVOLVE:
                consumedResource = resources[0];
                producedResource = resources[(int)Type];
                break;
            case ActionType.TRAIN:
                consumedResource = resources[0];
                producedResource = resources[(int)Type];
                break;
        }

        consumedResource.Consume(Consumption);
        producedResource.Produce(actualIncome);
    }

    public bool IsPerformable(Resource consumedResource)
    {
        return consumedResource.Storage - Consumption > 0;
    }
}
