﻿using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ResourceStatusPanel : MonoBehaviour
{
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
        populationText.text = $"{resources[(int)ResourceType.Population].GetStorageOverview()}";
        foodText.text       = $"{resources[(int)ResourceType.Food].GetStorageOverview()}";
        DNAText.text        = $"{resources[(int)ResourceType.DNA].GetStorageOverview()}";
        powerText.text      = $"{resources[(int)ResourceType.Power].GetStorageOverview()}";
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
