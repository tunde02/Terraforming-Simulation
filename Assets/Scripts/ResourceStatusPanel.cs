using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class ResourceStatusPanel : MonoBehaviour
{
    [SerializeField] private Text[] resourceTexts;
    [SerializeField] private Text[] resourceVariationTexts;
    [SerializeField] private float variationTime = 2f;

    private GameManager gameManager;


    [Inject]
    public void Construct(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    void Awake()
    {
        Resource.OnStorageSet += SetResourceText;
        Resource.OnProduced += UpdateResourceText;
        Resource.OnConsumed += UpdateResourceText;
    }

    private void SetResourceText(Resource resource)
    {
        resourceTexts[(int)resource.Type].text = $"{GetOverview(resource.Storage)}";
    }

    private void UpdateResourceText(Resource resource, long prev)
    {
        long variation = resource.Storage - prev;

        if (variation != 0)
        {
            PlayVariationTextAnimation(resourceVariationTexts[(int)resource.Type], variation);
        }
    }

    private string GetOverview(long number)
    {
        int unitIndex = 0;
        double compare = 1000;

        while (number >= compare)
        {
            compare *= 1000;
            unitIndex++;
        }

        return $"{Math.Floor(number / (compare / 1000) * 10) * 0.1d}{gameManager.UNIT[unitIndex]}";
    }

    private void PlayVariationTextAnimation(Text targetText, long amount)
    {
        targetText.text = $"{(amount > 0 ? "+" : "")} {GetOverview(amount)}";
        targetText.color = amount > 0 ? Color.green : Color.red;

        DOTween.Sequence().
            Append(targetText.DOFade(1, 0f).SetEase(Ease.InQuad)).
            Append(targetText.rectTransform.DOAnchorPosY(305f, variationTime)).
            Join(targetText.DOFade(0, variationTime).SetEase(Ease.InQuad)).
            Append(targetText.rectTransform.DOAnchorPosY(290f, 0f));
    }
}
