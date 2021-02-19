using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ActionManager : BaseManager
{
    [SerializeField] private GameObject scenarioDetailsCanvas;
    [SerializeField] private GameObject[] slotPrefabs;
    [SerializeField] private Transform[] slotPositions;

    public delegate void ScenarioHandler(List<ActionSlot> scenario);
    public static event ScenarioHandler OnScenarioChanged;

    public List<Action> ACTION { get; private set; } = new List<Action>(4) {
            new Action(ActionType.BREED),
            new Action(ActionType.HUNT),
            new Action(ActionType.EVOLVE),
            new Action(ActionType.TRAIN)
    };
    public List<ActionSlot> Scenario { get; private set; }
    public List<ActionSlot> PrevScenario { get; private set; }
    private GameManager gameManager;
    private UIManager uiManager;
    private readonly double[] blockWeight = { 1.0, 1.1, 1.2 };
    private readonly int blockStackLimit = 2;


    [Inject]
    public void Construct(GameManager gameManager, UIManager uiManager)
    {
        this.gameManager = gameManager;
        this.uiManager = uiManager;
    }

    public override void Initialize()
    {
        InitScenario();
        UpdateSlotWeights();
    }

    private void InitScenario()
    {
        int slotSize = gameManager.SLOTSIZE;

        Scenario = new List<ActionSlot>(slotSize);

        for (int i = 0; i < slotSize; i++)
            Scenario.Add(new ActionSlot(ACTION[0]));

        for (int i = gameManager.LockedIndex; i < slotSize; i++)
        {
            Scenario[i].IsEmpty = true;
            Scenario[i].IsLocked = true;
        }

        //OnScenarioChanged(Scenario);
    }

    public void SavePrevScenario()
    {
        PrevScenario = Scenario.ConvertAll(slot => new ActionSlot(slot.PlacedAction, slot.IsEmpty, slot.IsLocked));
    }

    public void InsertSlot(int index, int actionType)
    {
        Scenario[index].PlacedAction = ACTION[actionType];
        Scenario[index].IsEmpty = false;

        UpdateSlotWeights();
        OnScenarioChanged(Scenario);
    }

    public void RemoveSlotAt(int index)
    {
        Scenario[index].PlacedAction = ACTION[0];
        Scenario[index].IsEmpty = true;

        UpdateSlotWeights();
        OnScenarioChanged(Scenario);
    }

    public void ResetScenario()
    {
        Scenario = PrevScenario.ConvertAll(slot => new ActionSlot(slot.PlacedAction, slot.IsEmpty, slot.IsLocked));

        UpdateSlotWeights();
        OnScenarioChanged(Scenario);
    }

    public void ClearScenario()
    {
        foreach (var slot in Scenario)
        {
            slot.PlacedAction = ACTION[0];
            slot.IsEmpty = true;
        }

        UpdateSlotWeights();
        OnScenarioChanged(Scenario);
    }

    //public bool IsPerformable(List<Resource> resources)
    //{
    //    List<Resource> expectedResources = new List<Resource>(resources);

    //    foreach (var element in Scenario)
    //    {
    //        if (!element.PlacedAction.IsPerformable(expectedResources[0]))
    //        {
    //            return false;
    //        }

    //        element.PlacedAction.PerformAction(expectedResources);
    //    }

    //    return true;
    //}

    private void UpdateSlotWeights()
    {
        int[] stacks = new int[gameManager.LockedIndex];
        ActionType nowAction, nextAction;
        int blockStack = 0;

        for (int i = 1; i < gameManager.LockedIndex; i++)
        {
            nowAction = Scenario[i - 1].PlacedAction.Type;
            nextAction = Scenario[i].PlacedAction.Type;

            if (nowAction != nextAction || Scenario[i].IsEmpty || blockStack >= blockStackLimit)
            {
                blockStack = 0;
            }
            else if (!Scenario[i - 1].IsEmpty && nowAction == nextAction)
            {
                blockStack++;
            }

            stacks[i] = blockStack;
        }

        for (int i = stacks.Length - 1; i >= 0; i--)
        {
            for (int j = 0; j <= stacks[i]; j++)
            {
                Scenario[i - j].BlockWeight = blockWeight[stacks[i]];
            }

            i -= stacks[i];
        }

        //string slotWeights = "";
        //foreach (var slot in Scenario)
        //    slotWeights += slot.BlockWeight + " / ";
        //Debug.Log(slotWeights);
    }
}
