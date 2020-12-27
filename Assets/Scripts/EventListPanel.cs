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
        menuStatus = MenuStatus.Close;

        rectTransform = GetComponent<RectTransform>();
    }

    public override void OpenSideMenu()
    {
        menuStatus = MenuStatus.Open;

        rectTransform
            .DOAnchorPosX(inScreenPosX, 0.5f)
            .SetEase(Ease.OutExpo);
    }

    public override void CloseSideMenu()
    {
        menuStatus = MenuStatus.Close;

        rectTransform
            .DOAnchorPosX(outScreenPosX, 0.5f)
            .SetEase(Ease.OutExpo);
    }
}
