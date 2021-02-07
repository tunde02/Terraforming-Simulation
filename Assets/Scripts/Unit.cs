using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class Unit : MonoBehaviour
{
    [SerializeField] private List<Image> damagedImageList;
    [SerializeField] public Text hpText;


    public UnitSpec Spec { get; set; }
    public Barracks BelongedBarracks { get; set; }
    public Barracks TargetBarracks { get; set; }
    private RectTransform rectTransform;


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void MoveTo(Vector2 targetPosition)
    {
        rectTransform
            .DOAnchorPos(targetPosition, 5f)
            .SetEase(Ease.Linear);
    }

    public void MoveToTarget()
    {
        //rectTransform
        //.DOAnchorPos(TargetBarracks.GetComponent<RectTransform>().anchoredPosition, 5f)
        //.SetEase(Ease.Linear);
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (Spec.Hp > 0 && TargetBarracks)
        {
            //rectTransform
            //.DOAnchorPos(TargetBarracks.GetComponent<RectTransform>().anchoredPosition, 5f)
            //.SetEase(Ease.Linear);
            //Debug.Log($"Target : {TargetBarracks.GetInstanceID()}, {TargetBarracks.GetComponent<RectTransform>().anchoredPosition}");
            rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, TargetBarracks.GetComponent<RectTransform>().anchoredPosition, 1.5f);
            yield return new WaitForSeconds(0.008f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Barracks") && collision.GetComponent<Barracks>().BelongedFaction != Spec.BelongedFaction)
        {
            collision.GetComponent<Barracks>().OnDamaged(Spec.AttackPower);
            rectTransform.DOKill();
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Unit") && collision.GetComponent<Unit>().Spec.BelongedFaction != Spec.BelongedFaction)
        {
            OnDamaged(collision.GetComponent<Unit>().Spec.AttackPower);
        }
    }

    public void OnDamaged(int attackPower)
    {
        Spec.Hp -= attackPower * (100 - Spec.DefensePower) / 100;
        hpText.text = Spec.Hp.ToString();

        if (Spec.Hp <= 0)
        {
            rectTransform.DOKill();
            Destroy(gameObject);
        }
    }
}
