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
    public Transform actionBundlePanel;
    public GameObject[] slotPrefabs;


    private readonly float slotStartX = -460;
    private readonly float slotEndX = 460;
    private readonly float slotY = 0;
    private readonly float slotHeight = 110;
    private readonly float offset = 8;
    private List<GameObject> actionSlotList;


    void Awake()
    {
        float slotWidth = (slotEndX - slotStartX - (10 - 1) * offset) / 10;

        actionSlotList = new List<GameObject>();

        for (int i = 0; i < 10; i++)
        {
            actionSlotList.Add(Instantiate(slotPrefabs[4], actionBundlePanel));
            actionSlotList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(slotStartX + slotWidth / 2 + i * (slotWidth + offset), slotY);
            actionSlotList[i].GetComponent<RectTransform>().sizeDelta = new Vector2(slotWidth, slotHeight);
        }
    }

    public void ChangeStartTurnBtnImageTo(string imgType)
    {
        startTurnBtn.ChangeBtnImageTo(imgType);
    }

    public void UpdateResourceStatusTexts(List<Resource> resources)
    {
        resourceStatusPanel.UpdateResourceTexts(resources);
    }

    public void UpdateResourceDetailsTexts(List<Resource> resources)
    {
        resourceDetailsPanel.UpdateResourceDetailsTexts(resources);
    }

    public void ShowVariationTexts(List<(ResourceType resourceType, long amount)> variations)
    {
        resourceStatusPanel.ShowVariationTextsAnimation(variations);
    }

    public void UpdateActionBundlePanel(List<ActionBundleElement> actionBundle, int slotCount)
    {
        float slotWidth = (slotEndX - slotStartX - (slotCount - 1) * offset) / slotCount;

        foreach (var slot in actionSlotList)
            Destroy(slot);

        actionSlotList = new List<GameObject>();

        for (int i = 0; i < slotCount; i++)
        {
            if (actionBundle[i].IsLocked)
                break;

            actionSlotList.Add(Instantiate(slotPrefabs[(int)actionBundle[i].Action.Type], actionBundlePanel));
            actionSlotList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(slotStartX + slotWidth / 2 + i * (slotWidth + offset), slotY);
            actionSlotList[i].GetComponent<RectTransform>().sizeDelta = new Vector2(slotWidth, slotHeight);
        }
    }

    public void UpdateActionBundlePanel(Turn turn)
    {
        float slotWidth = (slotEndX - slotStartX - (turn.LockIndex - 1) * offset) / turn.LockIndex;

        foreach (var slot in actionSlotList)
            Destroy(slot);

        actionSlotList = new List<GameObject>();

        for (int i = 0; i < turn.LockIndex; i++)
        {
            if (turn.actionBundle[i].IsLocked)
                break;

            actionSlotList.Add(Instantiate(slotPrefabs[(int)turn.actionBundle[i].Action.Type], actionBundlePanel));
            actionSlotList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(slotStartX + slotWidth / 2 + i * (slotWidth + offset), slotY);
            actionSlotList[i].GetComponent<RectTransform>().sizeDelta = new Vector2(slotWidth, slotHeight);
        }
    }
}
