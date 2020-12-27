using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EventListPanel : SideMenuPanel
{
    private RectTransform rectTransform;
    private readonly float inScreenPosX = 150f;
    private readonly float outScreenPosX = 935f;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public override void MoveIntoScreen()
    {
        rectTransform
            .DOAnchorPosX(inScreenPosX, 0.5f)
            .SetEase(Ease.OutExpo);
    }

    public override void MoveOutOfScreen()
    {
        rectTransform
            .DOAnchorPosX(outScreenPosX, 0.5f)
            .SetEase(Ease.InExpo);
    }
}
