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
        nowTurn = new Turn(new List<Resource>(), new List<ActionBundleElement>(), 10, turnPeriodSecond);
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
                    gameManager.UpdateResources(nowTurn.resultResources);
                }

                if (nowActionIndex >= nowTurn.LockIndex)
                {
                    FinishTurn();
                    StartTurn();
                }

                break;
            case TurnStatus.Pause:
                break;
        }
    }

    public void StartTurn()
    {
        nowTurn = new Turn(gameManager.resources, actionManager.ActionBundle, actionManager.LockIndex, turnPeriodSecond)
        {
            Status = TurnStatus.Play,
            PlayTime = 0f
        };

        prevTime = Time.time;
        nowActionIndex = 0;

        uiManager.UpdateActionBundlePanel(nowTurn);
        startTurnBtn.ChangeBtnImageTo("PAUSE");

        StartTurnGauageAnimation(turnPeriodSecond);
    }

    public void PauseTurn()
    {
        if (nowTurn.Status != TurnStatus.Play)
            return;

        nowTurn.Status = TurnStatus.Pause;

        startTurnBtn.ChangeBtnImageTo("PLAY");

        StopTurnGaugeAnimation();
    }

    public void ResumeTurn()
    {
        if (nowTurn.Status != TurnStatus.Pause)
            return;

        nowTurn.Status = TurnStatus.Play;
        prevTime = Time.time;

        startTurnBtn.ChangeBtnImageTo("PAUSE");

        StartTurnGauageAnimation(nowTurn.Period - nowTurn.PlayTime);
    }

    public void FinishTurn()
    {
        nowTurn.Status = TurnStatus.Wait;

        ResetTurnGauge();
    }

    // 지울 예정
    public void ResetTurn()
    {
        Debug.Log("RESET TURN");

        nowTurn.Status = TurnStatus.Wait;
        nowTurn.PlayTime = 0f;

        gameManager.UpdateResources(nowTurn.startResources);

        startTurnBtn.ChangeBtnImageTo("PLAY");

        StopTurnGaugeAnimation();
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
