using System.Collections;
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

    public float variationTime = 2f;

    private Text[] resourceTexts;
    private Text[] variationTexts;
    private Sequence[] variations;
    
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

        // 이 밑, 개선 필요.
        variations = new Sequence[4];

        variations[0] = DOTween.Sequence()
                .SetAutoKill(false)
                .OnStart(() => {
                    variationTexts[0].rectTransform.DOAnchorPosY(290f, 0f);
                })
                .Append(variationTexts[0].rectTransform.DOAnchorPosY(305f, variationTime))
                .Join(variationTexts[0].DOFade(0, variationTime).SetEase(Ease.InQuad));

        variations[1] = DOTween.Sequence()
                .SetAutoKill(false)
                .OnStart(() => {
                    variationTexts[1].rectTransform.DOAnchorPosY(290f, 0f);
                })
                .Append(variationTexts[1].rectTransform.DOAnchorPosY(305f, variationTime))
                .Join(variationTexts[1].DOFade(0, variationTime).SetEase(Ease.InQuad));

        variations[2] = DOTween.Sequence()
                .SetAutoKill(false)
                .OnStart(() => {
                    variationTexts[2].rectTransform.DOAnchorPosY(290f, 0f);
                })
                .Append(variationTexts[2].rectTransform.DOAnchorPosY(305f, variationTime))
                .Join(variationTexts[2].DOFade(0, variationTime).SetEase(Ease.InQuad));

        variations[3] = DOTween.Sequence()
                .SetAutoKill(false)
                .OnStart(() => {
                    variationTexts[3].rectTransform.DOAnchorPosY(290f, 0f);
                })
                .Append(variationTexts[3].rectTransform.DOAnchorPosY(305f, variationTime))
                .Join(variationTexts[3].DOFade(0, variationTime).SetEase(Ease.InQuad));
    }

    public void UpdateResourceTexts(Resource[] resources)
    {
        for (int i=0; i<4; i++)
        {
            resourceTexts[i].text = $"{resources[i].storageOverview} + {resources[i].incomeOverview}";
            variationTexts[i].text = $"+{resources[i].incomeOverview}";
        }

        ShowVariationTextsAnimation();
    }

    private void ShowVariationTextsAnimation()
    {
        for (int i = 0; i < 4; i++)
            variations[i].Restart();
    }
}
