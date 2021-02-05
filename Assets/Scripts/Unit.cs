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
    private RectTransform rectTransform;


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void MoveTo(Vector2 targetPosition)
    {
        Debug.Log($"Move to {targetPosition}");
        rectTransform
            .DOAnchorPos(targetPosition, 5f)
            .SetEase(Ease.Linear);
    }

    public void MoveToTarget()
    {
        Debug.Log($"Move to target {Spec.TargetBarracks.transform}");

        rectTransform
            .DOAnchorPos(Spec.TargetBarracks.GetComponent<RectTransform>().anchoredPosition, 5f)
            .SetEase(Ease.Linear);
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
        Debug.Log("boom!!");
        Spec.Hp -= attackPower * (100 - Spec.DefensePower) / 100;
        hpText.text = Spec.Hp.ToString();

        if (Spec.Hp <= 0)
        {
            rectTransform.DOKill();
            Destroy(gameObject);
        }
    }
}
