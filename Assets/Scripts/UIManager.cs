using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public ResourceStatusPanel resourceStatusUI;
    public StartTurnBtn startTurnBtn;

    public void UpdateResourceTexts(Resource[] resources)
    {
        resourceStatusUI.UpdateResourceTexts(resources);
    }

    public void ChangeStartTurnBtnImageTo(string imgType)
    {
        startTurnBtn.ChangeBtnImageTo(imgType);
    }
}
