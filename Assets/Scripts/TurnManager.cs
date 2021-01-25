using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class TurnManager : MonoBehaviour
{
    public Turn NowTurn { get; set; }
    private GameManager gameManager;
    private ActionManager actionManager;
    private float prevTime;


    [Inject]
    public void Construct(GameManager gameManager, ActionManager actionManager)
    {
        this.gameManager = gameManager;
        this.actionManager = actionManager;
    }

    void Start()
    {
        NowTurn = new Turn(gameManager.Resources, actionManager.Scenario, gameManager.LockedIndex, gameManager.TURNPERIOD);
        Turn.OnTurnStatusChanged += ManageTurn;
    }

    void Update()
    {
        if (NowTurn.Status == TurnStatus.PLAYING)
        {
            NowTurn.PlayTime += Time.time - prevTime;
            prevTime = Time.time;
        }
    }

    private void ManageTurn(Turn turn, TurnStatus prevStatus)
    {
        if (prevStatus == TurnStatus.PLAYING && turn.Status == TurnStatus.WAITING)
        {
            StartTurn();
        }
    }

    public void StartTurn()
    {
        NowTurn = new Turn(gameManager.Resources, actionManager.Scenario, gameManager.LockedIndex, gameManager.TURNPERIOD)
        {
            Status = TurnStatus.PLAYING
        };

        prevTime = Time.time;
    }

    public void PauseTurn()
    {
        if (NowTurn.Status != TurnStatus.PLAYING)
            return;

        NowTurn.Status = TurnStatus.PAUSED;
    }

    public void ResumeTurn()
    {
        if (NowTurn.Status != TurnStatus.PAUSED)
            return;

        NowTurn.Status = TurnStatus.PLAYING;
        prevTime = Time.time;
    }

    //public void FinishTurn()
    //{
    //    NowTurn.Status = TurnStatus.WAITING;
    //}

    // 지울 예정
    //public void ResetTurn()
    //{
    //    Debug.Log("RESET TURN");

    //    NowTurn.Status = TurnStatus.WAITING;
    //    NowTurn.PlayTime = 0f;

    //    GameManager.Instance.UpdateResources(NowTurn.StartingResources);

    //    UIManager.Instance.StopTurnGaugeAnimation();
    //    UIManager.Instance.ResetTurnGauge();
    //}
}
