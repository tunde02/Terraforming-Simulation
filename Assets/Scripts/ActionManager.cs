using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionManager : MonoBehaviour
{
    public UIManager uiManager;
    public GameObject ActionBundleDetailsCanvas;
    public GameObject[] slotPrefabs;
    public Transform[] elementPositions;
    public GameObject notEnoughSlotsWarningWindow;
    public GameObject noEmptySlotWarningWindow;
    public GameObject unsavedWarningWindow;
    public float offset = 3.0f;


    private readonly List<Action> ACTION = new List<Action>(4) {
            new Action(ActionType.Breed),
            new Action(ActionType.Hunt),
            new Action(ActionType.Evolve),
            new Action(ActionType.Train)
    };
    public List<ActionBundleElement> ActionBundle { get; set; }
    private List<ActionBundleElement> prevActionBundle;
    private List<GameObject> elementObjects;
    private readonly int SLOTSIZE = 14;
    public int LockIndex { get; set; } = 10;
    private int topIndex;


    void Awake()
    {
        InitActionBundle();
        InitElementObjects();
    }

    private void InitActionBundle()
    {
        ActionBundle = new List<ActionBundleElement>(SLOTSIZE);

        for (int i = 0; i < SLOTSIZE; i++)
            ActionBundle.Add(new ActionBundleElement());

        for (int i = LockIndex; i < SLOTSIZE; i++)
            ActionBundle[i].IsLocked = true;

        UpdateTopIndex();
    }

    private void InitElementObjects()
    {
        elementObjects = new List<GameObject>(14);

        for (int i = 0; i < LockIndex; i++)
            elementObjects.Add(null);

        for (int i = LockIndex; i < SLOTSIZE; i++)
            elementObjects.Add(Instantiate(slotPrefabs[4], elementPositions[i]));
    }

    public void LoadActionBundleDetailsPanel()
    {
        prevActionBundle = ActionBundle.ConvertAll(a => new ActionBundleElement(a.PlacedAction, a.IsEmpty, a.IsLocked));

        for (int i = 0; i < SLOTSIZE; i++)
        {
            Destroy(elementObjects[i]);

            if (ActionBundle[i].IsLocked)
            {
                elementObjects[i] = Instantiate(slotPrefabs[4], elementPositions[i]);
            }
            else if (ActionBundle[i].IsEmpty)
            {
                elementObjects[i] = null;
            }
            else
            {
                int removeIndex = i;

                elementObjects[i] = Instantiate(slotPrefabs[(int)ActionBundle[i].PlacedAction.Type], elementPositions[i]);
                elementObjects[i].GetComponentInChildren<Button>().onClick.AddListener(() => RemoveBundleElementAt(removeIndex));
            }
        }

        UpdateTopIndex();
    }

    public void FillBundleElement(int actionId)
    {
        if (topIndex >= ActionBundle.Count || ActionBundle[topIndex].IsLocked)
        {
            uiManager.ShowAlertWindow("No More Empty Slot");
        }
        else
        {
            int removeIndex = topIndex;

            elementObjects[topIndex] = Instantiate(slotPrefabs[actionId], elementPositions[topIndex]);
            elementObjects[topIndex].GetComponentInChildren<Button>().onClick.AddListener(() => RemoveBundleElementAt(removeIndex));

            ActionBundle[topIndex].PlacedAction = ACTION[actionId];
            ActionBundle[topIndex].IsEmpty = false;

            UpdateTopIndex();
        }
    }

    public void RemoveBundleElementAt(int index)
    {
        Destroy(elementObjects[index]);
        elementObjects[index] = null;

        ActionBundle[index].IsEmpty = true;

        UpdateTopIndex();
    }

    private void UpdateTopIndex()
    {
        topIndex = 0;
        while (topIndex < ActionBundle.Count && !ActionBundle[topIndex].IsEmpty)
            topIndex++;
    }

    public void UnlockSlot()
    {
        Destroy(elementObjects[LockIndex]);
        elementObjects[LockIndex] = null;

        ActionBundle[LockIndex].IsLocked = false;
        prevActionBundle[LockIndex].IsLocked = false;

        LockIndex++;
    }

    public void SaveActionBundle()
    {
        // Action Bundle이 전부 채워져있다면
        // Action Bundle UI에 채워진 Action들을 actionBundle 리스트에 저장
        // 그렇지 않다면 경고창 띄워줌

        for (int i=0; i<LockIndex; i++)
        {
            if (ActionBundle[i].IsEmpty)
            {
                uiManager.ShowAlertWindow("Not Enough Slots");
                return;
            }
        }

        //uiManager.UpdateActionBundlePanel(ActionBundle, LockIndex);

        prevActionBundle = ActionBundle;
        ActionBundleDetailsCanvas.SetActive(false);
    }

    public void ClearActionBundle()
    {
        for (int i = 0; i < LockIndex; i++)
        {
            if (elementObjects[i])
            {
                Destroy(elementObjects[i]);
                elementObjects[i] = null;
            }

            ActionBundle[i].IsEmpty = true;
        }

        UpdateTopIndex();
    }

    public void ResetActionBundle()
    {
        ActionBundle = prevActionBundle;
    }

    public void PerformActionAt(Turn nowTurn, int actionIndex)
    {
        var nowAction = nowTurn.ActionBundle[actionIndex].PlacedAction;
        var variations = nowAction.PerformAction(nowTurn.resultResources[0], nowTurn.resultResources[(int)nowAction.Type]);

        uiManager.UpdateResourceStatusTexts(nowTurn.resultResources);
        uiManager.UpdateResourceDetailsTexts(nowTurn.resultResources);
        uiManager.ShowVariationTexts(variations);
    }

    public bool IsPerformable(List<Resource> resources)
    {
        List<Resource> expectedResources = new List<Resource>(resources);

        foreach (var element in ActionBundle)
        {
            if (!element.PlacedAction.IsPerformable(expectedResources[0]))
            {
                return false;
            }

            element.PlacedAction.PerformAction(expectedResources[0], expectedResources[(int)element.PlacedAction.Type]);
        }

        return true;
    }

    public void FillActionBundleByPreset()
    {
        ActionBundle[0] = new ActionBundleElement(ACTION[1]);
        ActionBundle[1] = new ActionBundleElement(ACTION[1]);
        ActionBundle[2] = new ActionBundleElement(ACTION[1]);
        ActionBundle[3] = new ActionBundleElement(ACTION[0]);
        ActionBundle[4] = new ActionBundleElement(ACTION[0]);
        ActionBundle[5] = new ActionBundleElement(ACTION[0]);
        ActionBundle[6] = new ActionBundleElement(ACTION[0]);
        ActionBundle[7] = new ActionBundleElement(ACTION[2]);
        ActionBundle[8] = new ActionBundleElement(ACTION[2]);
        ActionBundle[9] = new ActionBundleElement(ACTION[3]);
    }
}
