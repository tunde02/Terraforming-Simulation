using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTurnBtn : MonoBehaviour
{
    public Sprite playBtnImage;
    public Sprite pauseBtnImage;

    private bool isTurnInProgress = false;

    public void OnClicked(GameManager gameManager)
    {
        if (!isTurnInProgress)
        {
            isTurnInProgress = true;

            gameManager.StartTurn();

            GetComponent<Image>().sprite = pauseBtnImage;
        }
    }
}
