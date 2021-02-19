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

        Turn.OnTurnStarted += ChangeButtonImageToPlaying;
        Turn.OnTurnPaused += ChangeButtonImageToPaused;
        Turn.OnTurnResumed += ChangeButtonImageToPlaying;
        Turn.OnTurnFinished += ChangeButtonImageToPaused;
    }

    private void ChangeButtonImageToPlaying()
    {
        buttonImage.sprite = playingImage;
    }

    private void ChangeButtonImageToPaused()
    {
        buttonImage.sprite = pausedImage;
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
