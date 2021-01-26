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


    [Inject]
    public void Construct(GameManager gameManager, UIManager uiManager)
    {
        this.gameManager = gameManager;
        this.uiManager = uiManager;
    }

    void Awake()
    {
        InitScenario();
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
    }

    public void SavePrevScenario()
    {
        PrevScenario = Scenario.ConvertAll(slot => new ActionSlot(slot.PlacedAction, slot.IsEmpty, slot.IsLocked));
    }

    public void InsertSlot(int index, int actionType)
    {
        Scenario[index].PlacedAction = ACTION[actionType];
        Scenario[index].IsEmpty = false;

        OnScenarioChanged(Scenario);
    }

    public void RemoveSlotAt(int index)
    {
        Scenario[index].PlacedAction = ACTION[0];
        Scenario[index].IsEmpty = true;

        OnScenarioChanged(Scenario);
    }

    public void ResetScenario()
    {
        Scenario = PrevScenario.ConvertAll(slot => new ActionSlot(slot.PlacedAction, slot.IsEmpty, slot.IsLocked));

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
}
