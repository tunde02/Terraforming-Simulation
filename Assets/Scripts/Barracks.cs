using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;


public class Barracks : MonoBehaviour
{
    [SerializeField] private Transform battleArea;
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private Barracks tempTarget;


    [BoxGroup("Value")] public Faction BelongedFaction;
    [BoxGroup("Value")] public int Hp;
    public int DefensePower { get; set; }
    public Barracks TargetBarracks { get; set; }
    public float UnitProducePeriod { get; set; }
    public Unit ProducingUnit { get; set; }
    private UnitSpec producingUnitSpec;
    private List<Image> damagedImageList;


    void Awake()
    {
        Hp = 100;
        DefensePower = 10;
        TargetBarracks = tempTarget;
        UnitProducePeriod = 1f;
        producingUnitSpec = new UnitSpec(BelongedFaction, 100, 50, 0, 10f, 10, TargetBarracks, null);
    }

    [Button(Name = "Produce Unit")]
    private void ProduceUnit()
    {
        var unit = Instantiate(unitPrefab, transform.position, new Quaternion(), battleArea).GetComponent<Unit>();

        unit.Spec = new UnitSpec(
            producingUnitSpec.BelongedFaction,
            producingUnitSpec.Hp,
            producingUnitSpec.AttackPower,
            producingUnitSpec.DefensePower,
            producingUnitSpec.Speed,
            producingUnitSpec.UnitSize,
            producingUnitSpec.TargetBarracks,
            producingUnitSpec.DamagedImageList
        );
        unit.hpText.text = unit.Spec.Hp.ToString();

        unit.MoveToTarget();
    }

    public void OnDamaged(int attackPower)
    {
        Debug.Log($"Unit's Attack Power : {attackPower}");

        Hp -= attackPower;

        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
