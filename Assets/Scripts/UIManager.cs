using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public ActionManager actionManager;
    public StartTurnBtn startTurnBtn;
    public ResourceStatusPanel resourceStatusPanel;
    public ResourceDetailsPanel resourceDetailsPanel;
    public GameObject actionBundleDetailsCanvas;
    public Transform actionBundlePanel;
    public GameObject[] slotPrefabs;
    public GameObject alertWindow;


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

    public void UpdateActionBundlePanel(Turn turn)
    {
        float slotWidth = (slotEndX - slotStartX - (turn.LockIndex - 1) * offset) / turn.LockIndex;

        foreach (var slot in actionSlotList)
            Destroy(slot);

        actionSlotList = new List<GameObject>();

        for (int i = 0; i < turn.LockIndex; i++)
        {
            if (turn.ActionBundle[i].IsLocked)
                break;

            actionSlotList.Add(Instantiate(slotPrefabs[(int)turn.ActionBundle[i].PlacedAction.Type], actionBundlePanel));
            actionSlotList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(slotStartX + slotWidth / 2 + i * (slotWidth + offset), slotY);
            actionSlotList[i].GetComponent<RectTransform>().sizeDelta = new Vector2(slotWidth, slotHeight);
        }
    }

    public void ShowAlertWindow(string alertType)
    {
        // TODO: add click sound
        GameObject alert = Instantiate(alertWindow, actionBundleDetailsCanvas.transform);
        Text alertContent = alert.GetComponentInChildren<Text>();
        Button leftButton = alert.GetComponentsInChildren<Button>()[0];
        Button rightButton = alert.GetComponentsInChildren<Button>()[1];

        switch (alertType)
        {
            case "No More Empty Slot":
                alertContent.text = "슬롯이 가득 찼습니다.";
                leftButton.gameObject.SetActive(false);
                rightButton.GetComponentInChildren<Text>().text = "확인";
                rightButton.onClick.AddListener(() => {
                    Destroy(alert);
                });
                break;
            case "Not Enough Slots":
                alertContent.text =  "슬롯이 다 채워지지 않았습니다.\n저장하지 않고 나가시겠습니까?";
                leftButton.onClick.AddListener(() => {
                    actionManager.ResetActionBundle();
                    actionBundleDetailsCanvas.SetActive(false);
                    Destroy(alert);
                });
                rightButton.onClick.AddListener(() => {
                    Destroy(alert);
                });
                break;
            case "Unsaved Scenario":
                break;
            case "Impossible Scenario":
                break;
            default:
                Debug.LogError("Invalid alertType : UIManager.cs - ShowAlert()");
                break;
        }
    }
}
