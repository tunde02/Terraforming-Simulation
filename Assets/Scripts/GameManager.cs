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

    private Resource[] resources;

    public UIManager _uiManager;

    // need readonly later
    public float periodSecond;

    private float startTime;


    void Start()
    {
        InitResources();

        _uiManager.UpdateResourceTexts(resources);

        // 턴 방식 구현하면 턴 시작될 때 실행
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time - startTime >= periodSecond)
        {
            UpdateResources();
            _uiManager.UpdateResourceTexts(resources);

            // 턴 방식 구현하면 지워야됨
            startTime = Time.time;
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
            resource.storage += resource.income++;
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
}
