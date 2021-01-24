using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ActionManager : MonoBehaviour
{
    [SerializeField] private GameObject scenarioDetailsCanvas;
    [SerializeField] private GameObject[] slotPrefabs;
    [SerializeField] private Transform[] slotPositions;

    readonly private List<Action> ACTION = new List<Action>(4) {
            new Action(ActionType.BREED),
            new Action(ActionType.HUNT),
            new Action(ActionType.EVOLVE),
            new Action(ActionType.TRAIN)
    };
    public List<ActionSlot> Scenario { get; private set; }
    private GameManager gameManager;
    private UIManager uiManager;
    private List<ActionSlot> prevScenario;
    private List<GameObject> slotObjects;
    private int top;


    [Inject]
    public void Construct(GameManager gameManager, UIManager uiManager)
    {
        this.gameManager = gameManager;
        this.uiManager = uiManager;
    }

    void Awake()
    {
        InitScenario();
        InitSlotObjects();
    }

    private void InitScenario()
    {
        int slotSize = gameManager.SlotSize;

        Scenario = new List<ActionSlot>(slotSize);

        for (int i = 0; i < slotSize; i++)
            Scenario.Add(new ActionSlot());

        for (int i = gameManager.LockedIndex; i < slotSize; i++)
            Scenario[i].IsLocked = true;

        UpdateTop();
    }

    private void InitSlotObjects()
    {
        slotObjects = new List<GameObject>(14);

        for (int i = 0; i < gameManager.LockedIndex; i++)
            slotObjects.Add(null);

        for (int i = gameManager.LockedIndex; i < gameManager.SlotSize; i++)
            slotObjects.Add(Instantiate(slotPrefabs[4], slotPositions[i]));
    }

    public void LoadScenarioDetailsPanel()
    {
        prevScenario = Scenario.ConvertAll(a => new ActionSlot(a.PlacedAction, a.IsEmpty, a.IsLocked));

        for (int i = 0; i < gameManager.SlotSize; i++)
        {
            Destroy(slotObjects[i]);

            if (Scenario[i].IsLocked)
            {
                slotObjects[i] = Instantiate(slotPrefabs[4], slotPositions[i]);
            }
            else if (Scenario[i].IsEmpty)
            {
                slotObjects[i] = null;
            }
            else
            {
                int removeIndex = i;

                slotObjects[i] = Instantiate(slotPrefabs[(int)Scenario[i].PlacedAction.Type], slotPositions[i]);
                slotObjects[i].GetComponentInChildren<Button>().onClick.AddListener(() => RemoveSlotAt(removeIndex));
            }
        }
        
        UpdateTop();
    }

    public void FillSlot(int actionType)
    {
        if (top >= Scenario.Count || Scenario[top].IsLocked)
        {
            uiManager.ShowAlertWindow("No More Empty Slot");
        }
        else
        {
            //UIManager.Instance.FillSlot(actionType);
            int removeIndex = top;

            slotObjects[top] = Instantiate(slotPrefabs[actionType], slotPositions[top]);
            slotObjects[top].GetComponentInChildren<Button>().onClick.AddListener(() => RemoveSlotAt(removeIndex));

            Scenario[top].PlacedAction = ACTION[actionType];
            Scenario[top].IsEmpty = false;

            UpdateTop();
        }
    }

    public void RemoveSlotAt(int index)
    {
        Destroy(slotObjects[index]);
        slotObjects[index] = null;

        Scenario[index].IsEmpty = true;

        UpdateTop();
    }

    private void UpdateTop()
    {
        top = 0;
        while (top < Scenario.Count && !Scenario[top].IsEmpty)
            top++;
    }

    public void UnlockSlot()
    {
        int lockedIndex = gameManager.LockedIndex;

        Destroy(slotObjects[lockedIndex]);
        slotObjects[lockedIndex] = null;

        Scenario[lockedIndex].IsLocked = false;
        prevScenario[lockedIndex].IsLocked = false;

        gameManager.LockedIndex++;
    }

    public void SaveScenario()
    {
        for (int i=0; i<gameManager.LockedIndex; i++)
        {
            if (Scenario[i].IsEmpty)
            {
                uiManager.ShowAlertWindow("Not Enough Slots");
                return;
            }
        }

        prevScenario = Scenario;
        scenarioDetailsCanvas.SetActive(false);
    }

    public void ClearScenario()
    {
        for (int i = 0; i < gameManager.LockedIndex; i++)
        {
            if (slotObjects[i])
            {
                Destroy(slotObjects[i]);
                slotObjects[i] = null;
            }

            Scenario[i].IsEmpty = true;
        }

        UpdateTop();
    }

    public void ResetScenario()
    {
        Scenario = prevScenario;
    }

    public bool IsPerformable(List<Resource> resources)
    {
        List<Resource> expectedResources = new List<Resource>(resources);

        foreach (var element in Scenario)
        {
            if (!element.PlacedAction.IsPerformable(expectedResources[0]))
            {
                return false;
            }

            element.PlacedAction.PerformAction(expectedResources);
        }

        return true;
    }

    public void FillActionBundleByPreset()
    {
        Scenario[0] = new ActionSlot(ACTION[1]);
        Scenario[1] = new ActionSlot(ACTION[1]);
        Scenario[2] = new ActionSlot(ACTION[1]);
        Scenario[3] = new ActionSlot(ACTION[0]);
        Scenario[4] = new ActionSlot(ACTION[0]);
        Scenario[5] = new ActionSlot(ACTION[0]);
        Scenario[6] = new ActionSlot(ACTION[0]);
        Scenario[7] = new ActionSlot(ACTION[2]);
        Scenario[8] = new ActionSlot(ACTION[2]);
        Scenario[9] = new ActionSlot(ACTION[3]);
    }
}
