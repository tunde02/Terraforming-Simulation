using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;


public enum TurnStatus
{
    Wait,
    Play,
    Pause
}

public class GameManager : MonoBehaviour
{
    [BoxGroup("Resources")] public long initialPopulationStorage;
    [BoxGroup("Resources")] public long initialFoodStorage;
    [BoxGroup("Resources")] public long initialDNAStorage;
    [BoxGroup("Resources")] public long initialPowerStorage;

    [BoxGroup("Incomes")] public long initialPopulationIncome;
    [BoxGroup("Incomes")] public long initialFoodIncome;
    [BoxGroup("Incomes")] public long initialDNAIncome;
    [BoxGroup("Incomes")] public long initialPowerIncome;

    [BoxGroup("Texts")] public Text populationText;
    [BoxGroup("Texts")] public Text foodText;
    [BoxGroup("Texts")] public Text DNAText;
    [BoxGroup("Texts")] public Text powerText;

    // Monitoring values
    [BoxGroup("Monitoring")] public long nowPopulationStorage;
    [BoxGroup("Monitoring")] public long nowPopulationIncome;
    [BoxGroup("Monitoring")] public long nowFoodStorage;
    [BoxGroup("Monitoring")] public long nowFoodIncome;
    [BoxGroup("Monitoring")] public long nowDNAStorage;
    [BoxGroup("Monitoring")] public long nowDNAIncome;
    [BoxGroup("Monitoring")] public long nowPowerStorage;
    [BoxGroup("Monitoring")] public long nowPowerIncome;
    [BoxGroup("Monitoring")] public float turnPlayTime;
    [BoxGroup("Monitoring")] public int nowActionIndex;

    public UIManager uiManager;
    public TurnManager turnManager;
    public float turnPeriodSecond;


    public TurnStatus turnStatus;
    private Resource[] resources;
    private Action[] actions;
    private List<ActionType> actionBundle;
    private float prevTime;
    

    private void Awake()
    {
        InitResources();
        InitActions();
        actionBundle = new List<ActionType>();
        turnStatus = TurnStatus.Wait;
    }

    void Start()
    {
        uiManager.UpdateResourceTexts(resources);

        FillTestActions();
    }

    void Update()
    {
        switch (turnStatus)
        {
            case TurnStatus.Wait:
                break;
            case TurnStatus.Play:
                turnPlayTime += Time.time - prevTime;
                prevTime = Time.time;

                if (turnPlayTime >= turnPeriodSecond / actionBundle.Count * (nowActionIndex + 1))
                {
                    PerformActionAt(nowActionIndex++);
                }

                if (nowActionIndex >= actionBundle.Count)
                {
                    FinishTurn();
                }

                break;
            case TurnStatus.Pause:
                break;
        }
    }

    private void InitResources()
    {
        resources = new Resource[4];

        resources[(int)ResourceType.Population] = new Resource(ResourceType.Population, "population", initialPopulationStorage, initialPopulationIncome);
        resources[(int)ResourceType.Food]       = new Resource(ResourceType.Food, "food", initialFoodStorage, initialFoodIncome);
        resources[(int)ResourceType.DNA]        = new Resource(ResourceType.DNA, "DNA", initialDNAStorage, initialDNAIncome);
        resources[(int)ResourceType.Power]      = new Resource(ResourceType.Power, "power", initialPowerStorage, initialPowerIncome);
    }

    private void InitActions()
    {
        actions = new Action[4];

        actions[0] = new Action(ActionType.Breed);
        actions[1] = new Action(ActionType.Hunt);
        actions[2] = new Action(ActionType.Evolve);
        actions[3] = new Action(ActionType.Train);
    }

    public void StartTurn()
    {
        Debug.Log("START TURN");

        turnStatus = TurnStatus.Play;

        prevTime = Time.time;
        turnPlayTime = 0f;
        nowActionIndex = 0;

        turnManager.StartTurnGauageAnimation(turnPeriodSecond);
    }

    public void PauseTurn()
    {
        Debug.Log("PAUSE TURN");

        turnStatus = TurnStatus.Pause;

        turnManager.StopTurnGaugeAnimation();
    }

    public void ResumeTurn()
    {
        Debug.Log("RESUME TURN");

        turnStatus = TurnStatus.Play;

        prevTime = Time.time;

        turnManager.StartTurnGauageAnimation(turnPeriodSecond - turnPlayTime);
    }

    public void FinishTurn()
    {
        Debug.Log("FINISH TURN");

        turnStatus = TurnStatus.Wait;

        uiManager.ChangeStartTurnBtnImageTo("PLAY");
        turnManager.ResetTurnGauge();
    }

    public void PerformActionAt(int actionIndex)
    {
        var nowAction = actionBundle[actionIndex];
        var variations = actions[(int)nowAction].PerformAction(resources[0], resources[(int)nowAction]);

        uiManager.UpdateResourceTexts(resources);
        uiManager.ShowVariationTexts(variations);
    }

    private bool IsPerformable(List<ActionType> _actionBundle)
    {
        var expectedResources = resources;

        foreach (var nowAction in _actionBundle)
        {
            actions[(int)nowAction].PerformAction(expectedResources[0], expectedResources[(int)nowAction]);

            foreach (var resource in expectedResources)
            {
                if (resource.Storage < 0)
                    return false;
            }
        }

        return true;
    }

    [Button(Name = "Fill Test Actions")]
    public void FillTestActions()
    {
        actionBundle.Add(ActionType.Hunt);
        actionBundle.Add(ActionType.Hunt);
        actionBundle.Add(ActionType.Hunt);
        actionBundle.Add(ActionType.Breed);
        actionBundle.Add(ActionType.Breed);
        actionBundle.Add(ActionType.Breed);
        actionBundle.Add(ActionType.Breed);
        actionBundle.Add(ActionType.Evolve);
        actionBundle.Add(ActionType.Evolve);
        actionBundle.Add(ActionType.Hunt);

        if (!IsPerformable(actionBundle))
        {
            Debug.LogError("NOT PERFOMABLE ACTION BUNDLE!!");
            actionBundle.Clear();
        }
    }
}
