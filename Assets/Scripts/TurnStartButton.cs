using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class TurnStartButton : MonoBehaviour
{
    [SerializeField] private Sprite playingImage;
    [SerializeField] private Sprite pausedImage;

    private TurnManager turnManager;
    private Image buttonImage;


    [Inject]
    public void Construct(TurnManager turnManager)
    {
        this.turnManager = turnManager;
    }

    void Awake()
    {
        buttonImage = GetComponent<Image>();
        Turn.OnTurnStatusChanged += ChangeButtonImage;
    }

    private void ChangeButtonImage(Turn turn, TurnStatus prevStatus)
    {
        if (turn.Status == TurnStatus.WAITING || turn.Status == TurnStatus.PAUSED)
        {
            buttonImage.sprite = pausedImage;
        }
        else if (turn.Status == TurnStatus.PLAYING)
        {
            buttonImage.sprite = playingImage;
        }
        else
        {
            Debug.LogError("Invalid TurnStatus change : TurnStartButton.cs - ChangeButtonImage()");
        }
    }

    public void OnClicked()
    {
        switch (turnManager.NowTurn.Status)
        {
            case TurnStatus.WAITING:
                turnManager.StartTurn();
                break;
            case TurnStatus.PLAYING:
                turnManager.PauseTurn();
                break;
            case TurnStatus.PAUSED:
                turnManager.ResumeTurn();
                break;
        }
    }
}
