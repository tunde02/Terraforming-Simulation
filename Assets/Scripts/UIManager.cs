using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public StartTurnBtn startTurnBtn;
    public ResourceStatusPanel resourceStatusPanel;
    public ResourceDetailsPanel resourceDetailsPanel;
    

    public void ChangeStartTurnBtnImageTo(string imgType)
    {
        startTurnBtn.ChangeBtnImageTo(imgType);
    }

    public void UpdateResourceStatusTexts(Resource[] resources)
    {
        resourceStatusPanel.UpdateResourceTexts(resources);
    }

    public void UpdateResourceDetailsTexts(Resource[] resources)
    {
        resourceDetailsPanel.UpdateResourceDetailsTexts(resources);
    }

    public void ShowVariationTexts(List<(ResourceType resourceType, long amount)> variations)
    {
        resourceStatusPanel.ShowVariationTextsAnimation(variations);
    }
}
