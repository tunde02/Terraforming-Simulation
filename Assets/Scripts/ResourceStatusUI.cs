using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ResourceStatusUI : MonoBehaviour
{
    [BoxGroup("Texts")] public Text populationText;
    [BoxGroup("Texts")] public Text foodText;
    [BoxGroup("Texts")] public Text DNAText;
    [BoxGroup("Texts")] public Text powerText;

    //[BoxGroup("Variations")] public Text populationStorageVariationText;
    //[BoxGroup("Variations")] public Text populationIncomeVariationText;
    //[BoxGroup("Variations")] public Text foodStorageVariationText;
    //[BoxGroup("Variations")] public Text foodIncomeVariationText;
    //[BoxGroup("Variations")] public Text DNAStorageVariationText;
    //[BoxGroup("Variations")] public Text DNAIncomeVariationText;
    //[BoxGroup("Variations")] public Text powerStorageVariationText;
    //[BoxGroup("Variations")] public Text powerIncomeVariationText;

    [BoxGroup("Variations")] public Text populationStorageVariationText;
    [BoxGroup("Variations")] public Text foodStorageVariationText;
    [BoxGroup("Variations")] public Text DNAStorageVariationText;
    [BoxGroup("Variations")] public Text powerStorageVariationText;

    private Text[] resourceTexts;
    private Text[] variationTexts;

    private void Start()
    {
        resourceTexts = new Text[] {
            populationText,
            foodText,
            DNAText,
            powerText
        };

        variationTexts = new Text[] {
            populationStorageVariationText,
            foodStorageVariationText,
            DNAStorageVariationText,
            powerStorageVariationText
        };
    }

    public void UpdateResourceTexts(Resource[] resources)
    {
        for (int i=0; i<4; i++)
        {
            resourceTexts[i].text = $"{resources[i].storageOverview} + {resources[i].incomeOverview}";
            variationTexts[i].text = resources[i].incomeOverview;
        }
    }
}
