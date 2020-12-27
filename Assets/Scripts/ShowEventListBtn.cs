using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowEventListBtn : MonoBehaviour
{
    public Sprite openBtnImage;
    public Sprite closeBtnImage;


    public void OnClicked(SideMenuPanel sideMenu)
    {
        switch (sideMenu.menuStatus)
        {
            case MenuStatus.Open:
                sideMenu.CloseSideMenu();
                ChangeBtnImageTo("OPEN");
                break;
            case MenuStatus.Close:
                sideMenu.OpenSideMenu();
                ChangeBtnImageTo("CLOSE");
                break;
        }
    }

    public void ChangeBtnImageTo(string imgType)
    {
        if (imgType == "OPEN")
        {
            GetComponent<Image>().sprite = openBtnImage;
        }
        else if (imgType == "CLOSE")
        {
            GetComponent<Image>().sprite = closeBtnImage;
        }
        else
        {
            Debug.LogError("Invalid imageName : ShowEventListBtn.cs - ChangeBtnImageTo()");
        }
    }
}
