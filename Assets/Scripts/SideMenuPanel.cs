using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class SideMenuPanel : MonoBehaviour
{
    public bool IsOpened { get; set; } = false;


    private RectTransform rectTransform;
    private readonly float inScreenPosX = 150f;
    private readonly float outScreenPosX = 970f;

    private void Start()
    {
        IsOpened = false;
        rectTransform = GetComponent<RectTransform>();
    }

    public void OpenSideMenu()
    {
        IsOpened = true;

        rectTransform
            .DOAnchorPosX(inScreenPosX, 0.5f)
            .SetEase(Ease.OutExpo);
    }
    public void CloseSideMenu()
    {
        IsOpened = false;

        rectTransform
            .DOAnchorPosX(outScreenPosX, 0.5f)
            .SetEase(Ease.OutExpo);
    }
}
