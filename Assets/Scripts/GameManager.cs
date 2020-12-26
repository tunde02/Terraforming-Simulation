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

    public UIManager _uiManager;
    public TurnManager _turnManager;
    public Button turnStartBtn;
    public float turnPeriodSecond;


    private Resource[] resources;
    private float startTime;
    private bool isTurnStarted;


    void Start()
    {
        InitResources();

        _uiManager.UpdateResourceTexts(resources);

        isTurnStarted = false;
    }

    void Update()
    {
        if (isTurnStarted && Time.time - startTime >= turnPeriodSecond)
        {
            EndTurn();
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

    private void UpdateResources()
    {
        foreach (Resource resource in resources)
        {
            resource.storage += resource.income;
            resource.income *= 2;
            resource.UpdateOverviews();
        }

        // Update monitoring values
        nowPopulationStorage = resources[0].storage;
        nowPopulationIncome  = resources[0].income;
        nowFoodStorage       = resources[1].storage;
        nowFoodIncome        = resources[1].income;
        nowDNAStorage        = resources[2].storage;
        nowDNAIncome         = resources[2].income;
        nowPowerStorage      = resources[3].storage;
        nowPowerIncome       = resources[3].income;
    }

    [Button(Name = "Start Turn")]
    public void StartTurn()
    {
        Debug.Log("turn start");

        isTurnStarted = true;
        turnStartBtn.interactable = false;

        startTime = Time.time;

        _turnManager.StartTurnGauageAnimation();
    }

    public void EndTurn()
    {
        Debug.Log("turn end");

        isTurnStarted = false;
        turnStartBtn.interactable = true;

        UpdateResources();
        _uiManager.UpdateResourceTexts(resources);
        _turnManager.ResetTurnGauge();
    }
}
