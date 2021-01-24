using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioDetailsPanel : MonoBehaviour
{
    public GameObject[] slotPrefabs;
    public Transform[] slotPositions;


    private List<GameObject> slotObjects;
    private int top;


    public void InitSlotObjects(int lockedIndex, int slotSize)
    {
        slotObjects = new List<GameObject>(14);

        for (int i = 0; i < lockedIndex; i++)
            slotObjects.Add(null);

        for (int i = lockedIndex; i < slotSize; i++)
            slotObjects.Add(Instantiate(slotPrefabs[4], slotPositions[i]));
    }

    //public void FillSlot(int actionType)
    //{
    //    if (top >= Scenario.Count || Scenario[top].IsLocked)
    //    {
    //        uiManager.ShowAlertWindow("No More Empty Slot");
    //    }
    //    else
    //    {
    //        int removeIndex = top;

    //        slotObjects[top] = Instantiate(slotPrefabs[actionType], slotPositions[top]);
    //        slotObjects[top].GetComponentInChildren<Button>().onClick.AddListener(() => RemoveSlotAt(removeIndex));

    //        Scenario[top].PlacedAction = ACTION[actionType];
    //        Scenario[top].IsEmpty = false;

    //        UpdateTop();
    //    }
    //}

    //public void RemoveSlotAt(int index)
    //{
    //    Destroy(slotObjects[index]);
    //    slotObjects[index] = null;

    //    Scenario[index].IsEmpty = true;

    //    UpdateTop();
    //}

    //private void UpdateTop()
    //{
    //    top = 0;
    //    while (top < Scenario.Count && !Scenario[top].IsEmpty)
    //        top++;
    //}

    //public void UnlockSlot()
    //{
    //    Destroy(slotObjects[LockedIndex]);
    //    slotObjects[LockedIndex] = null;

    //    Scenario[LockedIndex].IsLocked = false;
    //    prevScenario[LockedIndex].IsLocked = false;

    //    LockedIndex++;
    //}

    //public void SaveScenario()
    //{
    //    for (int i = 0; i < LockedIndex; i++)
    //    {
    //        if (Scenario[i].IsEmpty)
    //        {
    //            uiManager.ShowAlertWindow("Not Enough Slots");
    //            return;
    //        }
    //    }

    //    //uiManager.UpdateActionBundlePanel(ActionBundle, LockIndex);

    //    prevScenario = Scenario;
    //    scenarioDetailsCanvas.SetActive(false);
    //}

    //public void ClearScenario()
    //{
    //    for (int i = 0; i < LockedIndex; i++)
    //    {
    //        if (slotObjects[i])
    //        {
    //            Destroy(slotObjects[i]);
    //            slotObjects[i] = null;
    //        }

    //        Scenario[i].IsEmpty = true;
    //    }

    //    UpdateTop();
    //}

    //public void ResetScenario()
    //{
    //    Scenario = prevScenario;
    //}

    //public void LoadSlotObjects(List<ActionSlot> scenario)
    //{
    //    var slotObjects = new List<GameObject>(scenario.Count);

    //    for (int i = 0; i < SLOTSIZE; i++)
    //    {
    //        Destroy(slotObjects[i]);

    //        if (Scenario[i].IsLocked)
    //        {
    //            slotObjects[i] = Instantiate(slotPrefabs[4], slotPositions[i]);
    //        }
    //        else if (Scenario[i].IsEmpty)
    //        {
    //            slotObjects[i] = null;
    //        }
    //        else
    //        {
    //            int removeIndex = i;

    //            slotObjects[i] = Instantiate(slotPrefabs[(int)Scenario[i].PlacedAction.Type], slotPositions[i]);
    //            slotObjects[i].GetComponentInChildren<Button>().onClick.AddListener(() => RemoveSlotAt(removeIndex));
    //        }
    //    }

    //    UpdateTop();
    //}
}
