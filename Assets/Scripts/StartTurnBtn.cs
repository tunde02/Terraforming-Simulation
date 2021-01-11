using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTurnBtn : MonoBehaviour
{
    public Sprite playBtnImage;
    public Sprite pauseBtnImage;


    public void OnClicked(TurnManager turnManager)
    {
        switch (turnManager.nowTurn.Status)
        {
            case TurnStatus.Wait:
                ChangeBtnImageTo("PAUSE");
                turnManager.StartTurn();

                break;
            case TurnStatus.Play:
                ChangeBtnImageTo("PLAY");
                turnManager.PauseTurn();

                break;
            case TurnStatus.Pause:
                ChangeBtnImageTo("PAUSE");
                turnManager.ResumeTurn();

                break;
        }
    }

    public void hello()
    {
        Debug.Log("hgege");
    }

    public void ChangeBtnImageTo(string imgType)
    {
        if (imgType == "PLAY")
        {
            GetComponent<Image>().sprite = playBtnImage;
        }
        else if (imgType == "PAUSE")
        {
            GetComponent<Image>().sprite = pauseBtnImage;
        }
        else
        {
            Debug.LogError("Invalid imageName : StartTurnBtn.cs - ChangeBtnImageTo()");
        }
    }
}
