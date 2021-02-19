using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : BaseManager
{
    [SerializeField] private BaseManager[] managers;

    [BoxGroup("Resources")] public long initialPopulationStorage;
    [BoxGroup("Resources")] public long initialFoodStorage;
    [BoxGroup("Resources")] public long initialDNAStorage;
    [BoxGroup("Resources")] public long initialPowerStorage;

    [BoxGroup("Monitoring")] public long nowPopulationStorage;
    [BoxGroup("Monitoring")] public long nowPopulationIncome;
    [BoxGroup("Monitoring")] public long nowFoodStorage;
    [BoxGroup("Monitoring")] public long nowFoodIncome;
    [BoxGroup("Monitoring")] public long nowDNAStorage;
    [BoxGroup("Monitoring")] public long nowDNAIncome;
    [BoxGroup("Monitoring")] public long nowPowerStorage;
    [BoxGroup("Monitoring")] public long nowPowerIncome;


    public List<Resource> Resources { get; set; }
    public string[] UNIT { get; } = { "", "k", "m", "g", "t", "p", "e" };
    public float TURNPERIOD { get; } = 10;
    public int SLOTSIZE { get; } = 14;
    public int LockedIndex { get; set; } = 10;
    

    public override void Initialize()
    {
        InitResources();
    }

    void Awake()
    {
        Initialize();

        InitOtherManagers();
    }

    private void InitResources()
    {
        Resources = new List<Resource>(4)
        {
            new Resource(ResourceType.POPULATION, initialPopulationStorage),
            new Resource(ResourceType.FOOD, initialFoodStorage),
            new Resource(ResourceType.DNA, initialDNAStorage),
            new Resource(ResourceType.POWER, initialPowerStorage)
        };
    }

    private void InitOtherManagers()
    {
        foreach (var manager in managers)
        {
            manager.Initialize();
        }
    }

    public void UpdateResources(List<Resource> resources)
    {
        for (int i = 0; i < resources.Count; i++)
            Resources[i].Storage = resources[i].Storage;

        UpdateMonitorings();
    }

    public void UpdateMonitorings()
    {
        nowPopulationStorage = Resources[0].Storage;
        nowFoodStorage = Resources[1].Storage;
        nowDNAStorage = Resources[2].Storage;
        nowPowerStorage = Resources[3].Storage;
    }
}
