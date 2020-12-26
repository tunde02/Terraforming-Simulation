using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTurnBtn : MonoBehaviour
{
    public Sprite playBtnImage;
    public Sprite pauseBtnImage;

    public void OnClicked(GameManager gameManager)
    {
        switch (gameManager.turnStatus)
        {
            case TurnStatus.Wait:
                ChangeBtnImageTo("PAUSE");
                gameManager.StartTurn();

                break;
            case TurnStatus.Play:
                ChangeBtnImageTo("PLAY");
                gameManager.PauseTurn();

                break;
            case TurnStatus.Pause:
                ChangeBtnImageTo("PAUSE");
                gameManager.ResumeTurn();

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
