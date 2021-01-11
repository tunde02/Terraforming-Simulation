using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [BoxGroup("Resources")] public long initialPopulationStorage;
    [BoxGroup("Resources")] public long initialFoodStorage;
    [BoxGroup("Resources")] public long initialDNAStorage;
    [BoxGroup("Resources")] public long initialPowerStorage;

    // Monitoring values
    [BoxGroup("Monitoring")] public long nowPopulationStorage;
    [BoxGroup("Monitoring")] public long nowPopulationIncome;
    [BoxGroup("Monitoring")] public long nowFoodStorage;
    [BoxGroup("Monitoring")] public long nowFoodIncome;
    [BoxGroup("Monitoring")] public long nowDNAStorage;
    [BoxGroup("Monitoring")] public long nowDNAIncome;
    [BoxGroup("Monitoring")] public long nowPowerStorage;
    [BoxGroup("Monitoring")] public long nowPowerIncome;

    public UIManager uiManager;
    public TurnManager turnManager;
    public ActionManager actionManager;
    public List<Resource> resources;
    

    void Awake()
    {
        InitResources();
    }

    void Start()
    {
        uiManager.UpdateResourceStatusTexts(resources);
        uiManager.UpdateResourceDetailsTexts(resources);
    }

    private void InitResources()
    {
        resources = new List<Resource>(4)
        {
            new Resource(ResourceType.Population, initialPopulationStorage),
            new Resource(ResourceType.Food, initialFoodStorage),
            new Resource(ResourceType.DNA, initialDNAStorage),
            new Resource(ResourceType.Power, initialPowerStorage)
        };
    }

    public void UpdateMonitorings(List<Action> actions)
    {
        nowPopulationStorage = resources[0].Storage;
        nowFoodStorage = resources[1].Storage;
        nowDNAStorage = resources[2].Storage;
        nowPowerStorage = resources[3].Storage;

        nowPopulationIncome = actions[0].Income;
        nowFoodIncome = actions[1].Income;
        nowDNAIncome = actions[2].Income;
        nowPowerIncome = actions[3].Income;
    }
}
