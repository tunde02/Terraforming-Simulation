using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class SideMenuPanel : MonoBehaviour
{
    public bool IsOpened { get; set; } = false;
    private RectTransform rectTransform;
    readonly private float openedPosX = 150f;
    readonly private float closedPosX = 970f;


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OpenSideMenu()
    {
        IsOpened = true;

        rectTransform
            .DOAnchorPosX(openedPosX, 0.5f)
            .SetEase(Ease.OutExpo);
    }
    public void CloseSideMenu()
    {
        IsOpened = false;

        rectTransform
            .DOAnchorPosX(closedPosX, 0.5f)
            .SetEase(Ease.OutExpo);
    }
}
