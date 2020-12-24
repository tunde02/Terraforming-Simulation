using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [BoxGroup("Resources")] public long populationStorage;
    [BoxGroup("Resources")] public long foodStorage;
    [BoxGroup("Resources")] public long DNAStorage;
    [BoxGroup("Resources")] public long powerStorage;

    [BoxGroup("Incomes")] public long populationIncome;
    [BoxGroup("Incomes")] public long foodIncome;
    [BoxGroup("Incomes")] public long DNAIncome;
    [BoxGroup("Incomes")] public long powerIncome;

    [BoxGroup("Texts")] public Text populationText;
    [BoxGroup("Texts")] public Text foodText;
    [BoxGroup("Texts")] public Text DNAText;
    [BoxGroup("Texts")] public Text powerText;


    private readonly string[] units = { "", "a", "b", "c", "d", "e", "f" };

    // need readonly later
    public float periodSecond;

    private float startTime;


    void Start()
    {
        UpdateResourcesTexts();

        // 턴 방식 구현하면 턴 시작될 때 실행
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time - startTime >= periodSecond)
        {
            UpdateResources();
            UpdateResourcesTexts();

            // 턴 방식 구현하면 지워야됨
            startTime = Time.time;
        }
    }

    private void UpdateResources()
    {
        populationStorage += populationIncome++;
        foodStorage += foodIncome++;
        DNAStorage += DNAIncome++;
        powerStorage += powerIncome++;
    }

    private void UpdateResourcesTexts()
    {
        long[] storages = { populationStorage, foodStorage, DNAStorage, powerStorage };
        long[] incomes = { populationIncome, foodIncome, DNAIncome, powerIncome };
        int[] storagesIndices = new int[4];
        int[] incomeIndices = new int[4];

        float[] resourceStorageFields = new float[4];
        float[] resourceIncomefields = new float[4];

        for (int i=0; i<storages.Length; i++)
        {
            while (storages[i] >= Mathf.Pow(1000, storagesIndices[i] + 1))
            {
                storagesIndices[i]++;
            }
        }

        for (int i = 0; i < incomes.Length; i++)
        {
            while (incomes[i] >= Mathf.Pow(1000, incomeIndices[i] + 1))
            {
                incomeIndices[i]++;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            resourceStorageFields[i] = InitField(storages[i], storagesIndices[i]);
            resourceIncomefields[i] = InitField(incomes[i], incomeIndices[i]);
        }

        populationText.text = $"{resourceStorageFields[0]}{units[storagesIndices[0]]} + {resourceIncomefields[0]}{units[incomeIndices[0]]}";
        foodText.text       = $"{resourceStorageFields[1]}{units[storagesIndices[1]]} + {resourceIncomefields[1]}{units[incomeIndices[1]]}";
        DNAText.text        = $"{resourceStorageFields[2]}{units[storagesIndices[2]]} + {resourceIncomefields[2]}{units[incomeIndices[2]]}";
        powerText.text      = $"{resourceStorageFields[3]}{units[storagesIndices[3]]} + {resourceIncomefields[3]}{units[incomeIndices[3]]}";

        /*
        float populationStorageField = Mathf.Floor(populationStorage / Mathf.Pow(1000, storagesIndices[0]) * 10) * 0.1f;
        float populationIncomField = Mathf.Floor(populationIncome / Mathf.Pow(1000, incomeIndices[0]) * 10) * 0.1f;
        float foodStorageField = Mathf.Floor(foodStorage / Mathf.Pow(1000, storagesIndices[1]) * 10) * 0.1f;
        float foodIncomField = Mathf.Floor(foodIncome / Mathf.Pow(1000, incomeIndices[1]) * 10) * 0.1f;
        float DNAStorageField = Mathf.Floor(DNAStorage / Mathf.Pow(1000, storagesIndices[2]) * 10) * 0.1f;
        float DNAIncomField = Mathf.Floor(DNAIncome / Mathf.Pow(1000, incomeIndices[2]) * 10) * 0.1f;
        float powerStorageField = Mathf.Floor(powerStorage / Mathf.Pow(1000, storagesIndices[3]) * 10) * 0.1f;
        float powerIncomField = Mathf.Floor(powerIncome / Mathf.Pow(1000, incomeIndices[3]) * 10) * 0.1f;

        populationText.text = $"{populationStorageField}{units[storagesIndices[0]]} + {populationIncomField}{units[incomeIndices[0]]}";
        foodText.text = $"{foodStorageField}{units[storagesIndices[1]]} + {foodIncomField}{units[incomeIndices[1]]}";
        DNAText.text = $"{DNAStorageField}{units[storagesIndices[2]]} + {DNAIncomField}{units[incomeIndices[2]]}";
        powerText.text = $"{powerStorageField}{units[storagesIndices[3]]} + {powerIncomField}{units[incomeIndices[3]]}";
        */
    }

    private float InitField(float value, int power)
    {
        return Mathf.Floor(value / Mathf.Pow(1000, power) * 10) * 0.1f;
    }
}
