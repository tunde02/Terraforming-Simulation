using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ScenarioDetailsPanel : MonoBehaviour
{
    [SerializeField] private GameObject scenarioDetailsCanvas;
    [SerializeField] private GameObject[] slotPrefabs;
    [SerializeField] private Transform[] slotPositions;
    [SerializeField] private Text[] resourceChangeTexts;

    private GameManager gameManager;
    private ActionManager actionManager;
    private UIManager uiManager;
    private List<GameObject> slotObjectList;
    private bool isSaved = true;
    private int top;


    [Inject]
    public void Construct(GameManager gameManager, ActionManager actionManager, UIManager uiManager)
    {
        this.gameManager = gameManager;
        this.actionManager = actionManager;
        this.uiManager = uiManager;
    }

    void Awake()
    {
        InitSlotObjectList();
        ActionManager.OnScenarioChanged += UpdateResourceChangeTexts;
    }

    void OnEnable()
    {
        actionManager.SavePrevScenario();

        LoadSlotObjectList(actionManager.Scenario);
        UpdateResourceChangeTexts(actionManager.Scenario);
    }

    private void InitSlotObjectList()
    {
        int lockedIndex = gameManager.LockedIndex;
        int slotSize = gameManager.SLOTSIZE;

        slotObjectList = new List<GameObject>(14);

        for (int i = 0; i < lockedIndex; i++)
            slotObjectList.Add(null);

        for (int i = lockedIndex; i < slotSize; i++)
            slotObjectList.Add(Instantiate(slotPrefabs[4], slotPositions[i]));
    }

    private void LoadSlotObjectList(List<ActionSlot> scenario)
    {
        for (int i = 0; i < scenario.Count; i++)
        {
            if (scenario[i].IsLocked) continue;

            Destroy(slotObjectList[i]);

            if (scenario[i].IsEmpty)
            {
                slotObjectList[i] = null;
            }
            else
            {
                int removeIndex = i;

                slotObjectList[i] = Instantiate(slotPrefabs[(int)scenario[i].PlacedAction.Type], slotPositions[i]);
                slotObjectList[i].GetComponentInChildren<Button>().onClick.AddListener(() => RemoveSlotAt(removeIndex));
            }
        }

        UpdateTop();
    }

    public void FillSlot(int actionType)
    {
        // Possible Errors
        // - Not Enough Slots Error

        if (top >= actionManager.Scenario.Count || actionManager.Scenario[top].IsLocked)
        {
            uiManager.ShowAlertWindow("No More Empty Slot");
        }
        else
        {
            int removeIndex = top;

            slotObjectList[top] = Instantiate(slotPrefabs[actionType], slotPositions[top]);
            slotObjectList[top].GetComponentInChildren<Button>().onClick.AddListener(() => RemoveSlotAt(removeIndex));

            actionManager.InsertSlot(top, actionType);

            isSaved = false;
            UpdateTop();
        }
    }

    public void RemoveSlotAt(int index)
    {
        Destroy(slotObjectList[index]);
        slotObjectList[index] = null;

        actionManager.RemoveSlotAt(index);

        isSaved = false;
        UpdateTop();
    }

    private void UpdateTop()
    {
        top = 0;
        while (top < actionManager.Scenario.Count && !actionManager.Scenario[top].IsEmpty)
            top++;
    }
    
    public void SaveScenario()
    {
        // Possible Errors
        // - Not Enough Slots Error
        // - Impossible Scenario Error

        for (int i = 0; i < gameManager.LockedIndex; i++)
        {
            if (actionManager.Scenario[i].IsEmpty)
            {
                uiManager.ShowAlertWindow("Not Enough Slots");
                return;
            }
        }

        uiManager.ShowAlertWindow("Saved Successfully");

        actionManager.SavePrevScenario();

        isSaved = true;
    }

    public void ClearSlots()
    {
        for (int i = 0; i < gameManager.LockedIndex; i++)
        {
            if (slotObjectList[i])
            {
                Destroy(slotObjectList[i]);
                slotObjectList[i] = null;
            }
        }

        actionManager.ClearScenario();

        isSaved = false;
        UpdateTop();
    }

    public void ResetScenario()
    {
        actionManager.ResetScenario();
        
        LoadSlotObjectList(actionManager.PrevScenario);

        isSaved = true;
        UpdateTop();
    }

    public void CloseScenarioDetailsWindow()
    {
        // Possible Errors
        // - Scenario Unsaved Error

        if (!isSaved)
        {
            uiManager.ShowAlertWindow("Scenario Unsaved");
        }
        else
        {
            scenarioDetailsCanvas.SetActive(false);
        }
    }

    public void FillActionBundleByPreset()
    {
        var scenario = actionManager.Scenario;
        var actions = actionManager.ACTION;

        scenario[0] = new ActionSlot(actions[1]);
        scenario[1] = new ActionSlot(actions[1]);
        scenario[2] = new ActionSlot(actions[1]);
        scenario[3] = new ActionSlot(actions[0]);
        scenario[4] = new ActionSlot(actions[0]);
        scenario[5] = new ActionSlot(actions[0]);
        scenario[6] = new ActionSlot(actions[0]);
        scenario[7] = new ActionSlot(actions[2]);
        scenario[8] = new ActionSlot(actions[2]);
        scenario[9] = new ActionSlot(actions[3]);
    }

    //public void UnlockSlot()
    //{
    //    Destroy(slotObjects[LockedIndex]);
    //    slotObjects[LockedIndex] = null;

    //    Scenario[LockedIndex].IsLocked = false;
    //    prevScenario[LockedIndex].IsLocked = false;

    //    LockedIndex++;
    //}

    private void UpdateResourceChangeTexts(List<ActionSlot> scenario)
    {
        long[] changes = new long[4];

        for (int i=0; i<gameManager.LockedIndex; i++)
        {
            if (scenario[i].IsEmpty) continue;

            var placedAction = scenario[i].PlacedAction;

            changes[(int)placedAction.Type] += (long)(placedAction.Income * scenario[i].BlockWeight);
            changes[0] -= placedAction.Consumption;
        }

        for (int i=0; i<4; i++)
        {
            resourceChangeTexts[i].text = $"{(changes[i] > 0 ? "+" : "")} {GetOverview(changes[i])}";
            //resourceChangeTexts[i].text = $"{(changes[i] > 0 ? "+" : "")} {changes[i]}";

            if (changes[i] < 0) resourceChangeTexts[i].color = Color.red;
            else if (changes[i] == 0) resourceChangeTexts[i].color = Color.black;
            else resourceChangeTexts[i].color = new Color(0, 188 / 255f, 21 / 255f);
        }
    }

    private string GetOverview(long number)
    {
        int unitIndex = 0;
        double compare = 1000;

        while (number >= compare)
        {
            compare *= 1000;
            unitIndex++;
        }

        return $"{Math.Floor(number / (compare / 1000) * 10) * 0.1d}{gameManager.UNIT[unitIndex]}";
    }
}
