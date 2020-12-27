﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideMenuBtn : MonoBehaviour
{
    public SideMenuPanel parentPanel;
    public RectTransform targetRectTransform;


    public void OnClicked()
    {
        if (!parentPanel.IsOpened)
        {
            parentPanel.OpenSideMenu();

            if (targetRectTransform.GetSiblingIndex() < 5)
            {
                targetRectTransform.SetAsLastSibling();
            }
        }
        else
        {
            // 맨 위의 패널이 targetPanel 이면 닫고, 아니면 시블링 인덱스 변경
            if (targetRectTransform.GetSiblingIndex() >= 5)
            {
                parentPanel.CloseSideMenu();
            }
            else
            {
                targetRectTransform.SetAsLastSibling();
            }
        }
    }
}
