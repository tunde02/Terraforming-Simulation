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
                turnManager.StartTurn();
                break;
            case TurnStatus.Play:
                turnManager.PauseTurn();
                break;
            case TurnStatus.Pause:
                turnManager.ResumeTurn();
                break;
        }
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
