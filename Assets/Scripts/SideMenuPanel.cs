using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MenuStatus
{
    Open,
    Close
}


public abstract class SideMenuPanel : MonoBehaviour
{
    public MenuStatus menuStatus;

    public abstract void OpenSideMenu();
    public abstract void CloseSideMenu();
}
