using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class TurnManager : BaseManager
{
    public Turn NowTurn { get; set; }
    public int TurnNumber { get; set; } = 0;
    private GameManager gameManager;
    private ActionManager actionManager;
    private bool isLoaded; // ProgressTurn() 대신 이거랑 Update() 같이 쓰는 것도 고려


    [Inject]
    public void Construct(GameManager gameManager, ActionManager actionManager)
    {
        this.gameManager = gameManager;
        this.actionManager = actionManager;
    }

    public override void Initialize()
    {
        InitTurn();

        StartCoroutine(ProgressTurn());
    }
    
    private void InitTurn()
    {
        NowTurn = new Turn(TurnStatus.WAITING, gameManager.Resources, actionManager.Scenario, gameManager.LockedIndex, gameManager.TURNPERIOD);

        Turn.OnTurnFinished += IncreaseTurnNumber;
        Turn.OnTurnFinished += ReadyTurn;
        Turn.OnTurnFinished += StartTurn;
    }

    private IEnumerator ProgressTurn()
    {
        while (true)
        {
            if (NowTurn.Status == TurnStatus.PLAYING)
            {
                NowTurn.PlayTime += Time.deltaTime;
            }

            yield return null;
        }
    }

    public void ReadyTurn()
    {
        NowTurn = new Turn(TurnStatus.WAITING, gameManager.Resources, actionManager.Scenario, gameManager.LockedIndex, gameManager.TURNPERIOD);
    }

    public void StartTurn()
    {
        //Debug.Log("start turn");
        //NowTurn = new Turn(TurnStatus.WAITING, gameManager.Resources, actionManager.Scenario, gameManager.LockedIndex, gameManager.TURNPERIOD);
        NowTurn.Status = TurnStatus.PLAYING;
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
    }

    private void IncreaseTurnNumber()
    {
        TurnNumber++;
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
