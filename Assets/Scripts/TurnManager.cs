using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public RawImage turnGauage;

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
