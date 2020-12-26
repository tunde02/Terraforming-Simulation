using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public RawImage turnGauage;

    public void StartTurnGauageAnimation()
    {
        turnGauage.rectTransform.DOScaleX(8500f, 5f).SetEase(Ease.Linear);
    }

    public void ResetTurnGauge()
    {
        turnGauage.rectTransform.DOScaleX(1f, 0f);
    }
}
