using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBundleElement
{
    public Action Action { get; set; }
    public bool IsEmpty { get; set; }
    public bool IsLocked { get; set; }


    public ActionBundleElement()
    {
        Action = new Action(ActionType.Breed);
        IsEmpty = true;
        IsLocked = false;
    }

    public ActionBundleElement(Action action)
    {
        Action = action;
        IsEmpty = false;
        IsLocked = false;
    }

    public ActionBundleElement(Action action, bool isEmpty, bool isLocked)
    {
        Action = action;
        IsEmpty = isEmpty;
        IsLocked = isLocked;
    }
}
