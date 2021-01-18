﻿using System.Collections;
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
    public float offset = 3.0f;


    private List<Action> actions;
    public List<ActionBundleElement> ActionBundle { get; set; }
    private List<ActionBundleElement> prevActionBundle;
    private List<GameObject> elementObjects;
    private readonly int SLOTSIZE = 14;
    public int LockIndex { get; set; } = 10;
    private int topIndex;


    void Awake()
    {
        InitActions();
        InitActionBundle();
        InitElementObjects();
    }

    public void LoadActionBundleDetailsPanel()
    {
        prevActionBundle = ActionBundle.ConvertAll(a => new ActionBundleElement(a.Action, a.IsEmpty, a.IsLocked));

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

                elementObjects[i] = Instantiate(slotPrefabs[(int)ActionBundle[i].Action.Type], elementPositions[i]);
                elementObjects[i].GetComponentInChildren<Button>().onClick.AddListener(() => RemoveBundleElementAt(removeIndex));
            }
        }

        UpdateTopIndex();
    }

    #region Initialize
    private void InitActions()
    {
        actions = new List<Action>(4)
        {
            new Action(ActionType.Breed),
            new Action(ActionType.Hunt),
            new Action(ActionType.Evolve),
            new Action(ActionType.Train)
        };
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
    #endregion

    public void FillBundleElement(int actionId)
    {
        if (topIndex >= ActionBundle.Count || ActionBundle[topIndex].IsLocked)
        {
            ShowNoEmptySlotWarning();
        }
        else
        {
            int removeIndex = topIndex;

            elementObjects[topIndex] = Instantiate(slotPrefabs[actionId], elementPositions[topIndex]);
            elementObjects[topIndex].GetComponentInChildren<Button>().onClick.AddListener(() => RemoveBundleElementAt(removeIndex));

            ActionBundle[topIndex].Action = actions[actionId];
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

    public void ResetActionBundle()
    {
        for (int i=0; i<LockIndex; i++)
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

    private void UpdateTopIndex()
    {
        topIndex = 0;
        while (topIndex < ActionBundle.Count && !ActionBundle[topIndex].IsEmpty)
            topIndex++;
    }

    public void ShowNoEmptySlotWarning()
    {
        noEmptySlotWarningWindow.SetActive(true);
    }

    public void ShowNotEnoughSlotsWarning()
    {
        notEnoughSlotsWarningWindow.SetActive(true);
    }

    public void UnlockBundleSlot()
    {
        Destroy(elementObjects[LockIndex]);
        elementObjects[LockIndex] = null;

        ActionBundle[LockIndex].IsLocked = false;
        prevActionBundle[LockIndex].IsLocked = false;

        LockIndex++;
    }

    public void UpdateActionBundle()
    {
        // Action Bundle이 전부 채워져있다면
        // Action Bundle UI에 채워진 Action들을 actionBundle 리스트에 추가
        // 그렇지 않다면 경고창 띄워줌

        for (int i=0; i<LockIndex; i++)
        {
            if (ActionBundle[i].IsEmpty)
            {
                ShowNotEnoughSlotsWarning();
                return;
            }
        }

        //uiManager.UpdateActionBundlePanel(ActionBundle, LockIndex);

        prevActionBundle = ActionBundle;
        ActionBundleDetailsCanvas.SetActive(false);
    }

    public void RevertActionBundle()
    {
        ActionBundle = prevActionBundle;
    }

    public void PerformActionAt(Turn nowTurn, int actionIndex)
    {
        var nowAction = nowTurn.actionBundle[actionIndex].Action;
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
            if (!element.Action.IsPerformable(expectedResources[0]))
            {
                return false;
            }

            element.Action.PerformAction(expectedResources[0], expectedResources[(int)element.Action.Type]);
        }

        return true;
    }

    public void FillActionBundleByPreset()
    {
        ActionBundle[0] = new ActionBundleElement(actions[1]);
        ActionBundle[1] = new ActionBundleElement(actions[1]);
        ActionBundle[2] = new ActionBundleElement(actions[1]);
        ActionBundle[3] = new ActionBundleElement(actions[0]);
        ActionBundle[4] = new ActionBundleElement(actions[0]);
        ActionBundle[5] = new ActionBundleElement(actions[0]);
        ActionBundle[6] = new ActionBundleElement(actions[0]);
        ActionBundle[7] = new ActionBundleElement(actions[2]);
        ActionBundle[8] = new ActionBundleElement(actions[2]);
        ActionBundle[9] = new ActionBundleElement(actions[3]);
    }
}
