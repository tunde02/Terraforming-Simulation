using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public enum TurnStatus
{
    Wait,
    Play,
    Pause
}

public class TurnManager : MonoBehaviour
{
    public UIManager uiManager;
    public GameManager gameManager;
    public ActionManager actionManager;
    public RawImage turnGauage;
    public StartTurnBtn startTurnBtn;
    public float turnPeriodSecond;


    public Turn nowTurn;
    private float prevTime;
    private int nowActionIndex;


    void Awake()
    {
        nowTurn = new Turn(new List<Resource>(), new List<ActionType>(), turnPeriodSecond);
    }

    void Update()
    {
        switch (nowTurn.Status)
        {
            case TurnStatus.Wait:
                break;
            case TurnStatus.Play:
                nowTurn.PlayTime += Time.time - prevTime;
                prevTime = Time.time;

                if (nowTurn.IsEnoughPlayTime(nowActionIndex))
                {
                    actionManager.PerformActionAt(nowTurn, nowActionIndex++);
                }

                if (nowActionIndex >= nowTurn.actionBundle.Count)
                {
                    FinishTurn();
                }

                break;
            case TurnStatus.Pause:
                break;
        }
    }

    public void StartTurn()
    {
        Debug.Log("START TURN");

        nowTurn = new Turn(gameManager.resources, actionManager.actionBundle, turnPeriodSecond)
        {
            Status = TurnStatus.Play,
            PlayTime = 0f
        };

        prevTime = Time.time;
        nowActionIndex = 0;

        StartTurnGauageAnimation(turnPeriodSecond);
    }

    public void PauseTurn()
    {
        Debug.Log("PAUSE TURN");

        nowTurn.Status = TurnStatus.Pause;

        StopTurnGaugeAnimation();
    }

    public void ResumeTurn()
    {
        Debug.Log("RESUME TURN");

        nowTurn.Status = TurnStatus.Play;

        prevTime = Time.time;

        StartTurnGauageAnimation(nowTurn.Period - nowTurn.PlayTime);
    }

    public void FinishTurn()
    {
        Debug.Log("FINISH TURN");

        nowTurn.Status = TurnStatus.Wait;

        startTurnBtn.ChangeBtnImageTo("PLAY");
        ResetTurnGauge();
    }

    public void StartTurnGauageAnimation(float duration)
    {
        turnGauage.rectTransform.DOScaleX(9300f, duration).SetEase(Ease.Linear);
    }

    public void StopTurnGaugeAnimation()
    {
        turnGauage.rectTransform.DOPause();
    }

    public void ResetTurnGauge()
    {
        turnGauage.rectTransform.DOScaleX(1f, 0f);
    }
}
