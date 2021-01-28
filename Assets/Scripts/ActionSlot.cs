using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSlot
{
    public Action PlacedAction { get; set; }
    public bool IsEmpty { get; set; }
    public bool IsLocked { get; set; }
    public double BlockWeight { get; set; } = 1.0;


    public ActionSlot()
    {
        PlacedAction = new Action(ActionType.BREED);
        IsEmpty = true;
        IsLocked = false;
    }

    public ActionSlot(Action action)
    {
        PlacedAction = action;
        IsEmpty = false;
        IsLocked = false;
    }

    public ActionSlot(Action action, bool isEmpty, bool isLocked)
    {
        PlacedAction = action;
        IsEmpty = isEmpty;
        IsLocked = isLocked;
    }
}
