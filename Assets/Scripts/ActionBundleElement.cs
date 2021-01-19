using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBundleElement
{
    public Action PlacedAction { get; set; }
    public bool IsEmpty { get; set; }
    public bool IsLocked { get; set; }


    public ActionBundleElement()
    {
        PlacedAction = new Action(ActionType.Breed);
        IsEmpty = true;
        IsLocked = false;
    }

    public ActionBundleElement(Action action)
    {
        PlacedAction = action;
        IsEmpty = false;
        IsLocked = false;
    }

    public ActionBundleElement(Action action, bool isEmpty, bool isLocked)
    {
        PlacedAction = action;
        IsEmpty = isEmpty;
        IsLocked = isLocked;
    }
}
