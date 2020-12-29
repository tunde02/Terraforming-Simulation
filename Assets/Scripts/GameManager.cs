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

    public UIManager _uiManager;
    public TurnManager _turnManager;
    public float turnPeriodSecond;


    public TurnStatus turnStatus;
    private Resource[] resources;
    private Action[] actions;
    private LinkedList<ActionType> actionBundle;
    private float prevTime;


    private void Awake()
    {
        InitResources();
        InitActions();
        actionBundle = new LinkedList<ActionType>();
        turnStatus = TurnStatus.Wait;
    }

    void Start()
    {
        _uiManager.UpdateResourceTexts(resources);

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

                if (turnPlayTime >= turnPeriodSecond)
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

        _turnManager.StartTurnGauageAnimation(turnPeriodSecond);

        StartCoroutine(PerformActionBundle());
    }

    public void PauseTurn()
    {
        Debug.Log("PAUSE TURN");

        turnStatus = TurnStatus.Pause;

        _turnManager.StopTurnGaugeAnimation();
    }

    public void ResumeTurn()
    {
        Debug.Log("RESUME TURN");

        turnStatus = TurnStatus.Play;

        prevTime = Time.time;

        _turnManager.StartTurnGauageAnimation(turnPeriodSecond - turnPlayTime);
    }

    public void FinishTurn()
    {
        Debug.Log("FINISH TURN");

        turnStatus = TurnStatus.Wait;
        turnPlayTime = 0f;

        _uiManager.ChangeStartTurnBtnImageTo("PLAY");
        _turnManager.ResetTurnGauge();
    }

    public IEnumerator PerformActionBundle()
    {
        var variations = new List<(ResourceType resourceType, long amount)>();

        foreach (ActionType actionType in actionBundle)
        {
            yield return new WaitForSeconds(turnPeriodSecond / actionBundle.Count);

            variations.AddRange(actions[(int)actionType].PerformAction(resources[0], resources[(int)actionType]));
            _uiManager.UpdateResourceTexts(resources);
            _uiManager.ShowVariationTexts(variations);

            variations.Clear();
        }
    }

    public void FillTestActions()
    {
        actionBundle.AddLast(ActionType.Hunt);
        actionBundle.AddLast(ActionType.Hunt);
        actionBundle.AddLast(ActionType.Hunt);
        actionBundle.AddLast(ActionType.Breed);
        actionBundle.AddLast(ActionType.Breed);
        actionBundle.AddLast(ActionType.Breed);
        actionBundle.AddLast(ActionType.Breed);
        actionBundle.AddLast(ActionType.Evolve);
        actionBundle.AddLast(ActionType.Evolve);
        actionBundle.AddLast(ActionType.Hunt);
    }
}
