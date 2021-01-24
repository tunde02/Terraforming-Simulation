using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SideMenuButton : MonoBehaviour
{
    [SerializeField] private SideMenuPanel parentPanel;
    [SerializeField] private RectTransform targetRectTransform;


    public void OnClicked()
    {
        if (!parentPanel.IsOpened)
        {
            parentPanel.OpenSideMenu();

            if (targetRectTransform.GetSiblingIndex() < 2)
            {
                targetRectTransform.SetSiblingIndex(2);
            }
        }
        else
        {
            // 맨 위의 패널이 targetPanel 이면 닫고, 아니면 시블링 인덱스 변경
            if (targetRectTransform.GetSiblingIndex() >= 2)
            {
                parentPanel.CloseSideMenu();
            }
            else
            {
                targetRectTransform.SetSiblingIndex(2);
            }
        }
    }
}
