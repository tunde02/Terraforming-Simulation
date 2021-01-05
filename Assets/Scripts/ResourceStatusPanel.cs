using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ResourceStatusPanel : MonoBehaviour
{
    private readonly string[] units = { "", "a", "b", "c", "d", "e", "f" };

    [BoxGroup("Texts")] public Text populationText;
    [BoxGroup("Texts")] public Text foodText;
    [BoxGroup("Texts")] public Text DNAText;
    [BoxGroup("Texts")] public Text powerText;

    [BoxGroup("Variations")] public Text populationStorageVariationText;
    [BoxGroup("Variations")] public Text foodStorageVariationText;
    [BoxGroup("Variations")] public Text DNAStorageVariationText;
    [BoxGroup("Variations")] public Text powerStorageVariationText;

    public float variationTime = 2f;


    public void UpdateResourceTexts(Resource[] resources)
    {
        populationText.text = $"{GetStorageOverview(resources[(int)ResourceType.Population].Storage)}";
        foodText.text       = $"{GetStorageOverview(resources[(int)ResourceType.Food].Storage)}";
        DNAText.text        = $"{GetStorageOverview(resources[(int)ResourceType.DNA].Storage)}";
        powerText.text      = $"{GetStorageOverview(resources[(int)ResourceType.Power].Storage)}";
    }

    private string GetStorageOverview(long storage)
    {
        int storageUnit = 0;
        double storageCompare = 1000;

        while (storage >= storageCompare)
        {
            storageCompare *= 1000;
            storageUnit++;
        }

        return $"{Math.Floor(storage / (storageCompare / 1000) * 10) * 0.1d}{units[storageUnit]}";
    }

    public void ShowVariationTextsAnimation(List<(ResourceType resourceType, long amount)> variations)
    {
        foreach (var (resourceType, amount) in variations)
        {
            switch (resourceType)
            {
                case ResourceType.Population:
                    MoveVariationText(populationStorageVariationText, amount);
                    break;
                case ResourceType.Food:
                    MoveVariationText(foodStorageVariationText, amount);
                    break;
                case ResourceType.DNA:
                    MoveVariationText(DNAStorageVariationText, amount);
                    break;
                case ResourceType.Power:
                    MoveVariationText(powerStorageVariationText, amount);
                    break;
            }
        }
    }

    private void MoveVariationText(Text targetText, long amount)
    {
        targetText.text = $"{(amount > 0 ? "+" : "")} {amount}";
        targetText.color = amount > 0 ? Color.green : Color.red;

        DOTween.Sequence().
            Append(targetText.DOFade(1, 0f).SetEase(Ease.InQuad)).
            Append(targetText.rectTransform.DOAnchorPosY(305f, variationTime)).
            Join(targetText.DOFade(0, variationTime).SetEase(Ease.InQuad)).
            Append(targetText.rectTransform.DOAnchorPosY(290f, 0f));
    }
}
