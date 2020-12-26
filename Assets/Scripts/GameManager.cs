using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public enum ResourceID
{
    Population = 0,
    Food = 1,
    DNA = 2,
    Power = 3
}

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
    private float prevTime;


    void Start()
    {
        InitResources();

        _uiManager.UpdateResourceTexts(resources);

        turnStatus = TurnStatus.Wait;
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

        resources[(int)ResourceID.Population] = new Resource(ResourceID.Population, "population", initialPopulationStorage, initialPopulationIncome);
        resources[(int)ResourceID.Food]       = new Resource(ResourceID.Food, "food", initialFoodStorage, initialFoodIncome);
        resources[(int)ResourceID.DNA]        = new Resource(ResourceID.DNA, "DNA", initialDNAStorage, initialDNAIncome);
        resources[(int)ResourceID.Power]      = new Resource(ResourceID.Power, "power", initialPowerStorage, initialPowerIncome);
    }

    private void UpdateResourcesStorage()
    {
        foreach (Resource resource in resources)
        {
            resource.storage += resource.income;
            resource.UpdateOverviews();
        }

        // Update monitoring values
        nowPopulationStorage = resources[0].storage;
        nowFoodStorage       = resources[1].storage;
        nowDNAStorage        = resources[2].storage;
        nowPowerStorage      = resources[3].storage;
    }

    [Button(Name="Update Income")]
    public void UpdateResourcesIncome()
    {
        foreach (Resource resource in resources)
        {
            resource.income *= 2;
            resource.UpdateOverviews();
        }

        // Update monitoring values
        nowPopulationIncome = resources[0].income;
        nowFoodIncome = resources[1].income;
        nowDNAIncome = resources[2].income;
        nowPowerIncome = resources[3].income;
    }

    [Button(Name = "Start Turn")]
    public void StartTurn()
    {
        Debug.Log("START TURN");

        turnStatus = TurnStatus.Play;

        prevTime = Time.time;
        turnPlayTime = 0f;

        _turnManager.StartTurnGauageAnimation(turnPeriodSecond);
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

        UpdateResourcesStorage();

        _uiManager.UpdateResourceTexts(resources);
        _uiManager.ChangeStartTurnBtnImageTo("PLAY");
        _turnManager.ResetTurnGauge();
    }
}
