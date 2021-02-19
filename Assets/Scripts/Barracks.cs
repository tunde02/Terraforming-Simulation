using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Zenject;


public class Barracks : MonoBehaviour
{
    //public delegate void TargetBarracksHandler(Barracks destroyedBarracks);
    //public static event TargetBarracksHandler OnTargetBarracksDestroyed;


    [SerializeField] private Transform battleArea;
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private float producePeriod;
    [SerializeField] private Text hpText;


    [BoxGroup("Values")] public Faction BelongedFaction;
    [BoxGroup("Values")] public int Hp;
    [BoxGroup("Unit Spec")] public int unitHp;
    [BoxGroup("Unit Spec")] public int unitAttackPower;
    [BoxGroup("Unit Spec")] public int unitDefensePower;
    public int DefensePower { get; set; }
    public Barracks TargetBarracks { get; set; }
    public RectTransform TargetBarracksPosition { get; set; }
    public float UnitProducePeriod { get; set; }
    public Unit ProducingUnit { get; set; }

    private BattleManager battleManager;
    public UnitSpec ProducingUnitSpec { get; set; }
    private List<Image> damagedImageList;
    private float timeStack;
    private List<Unit> unitList;


    [Inject]
    public void Construct(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    void Awake()
    {
        //Hp = 100;
        DefensePower = 10;
        TargetBarracks = null;
        //TargetBarracksPosition = target1.GetComponent<RectTransform>();
        UnitProducePeriod = 1f;
        ProducingUnitSpec = new UnitSpec(BelongedFaction, unitHp, unitAttackPower, unitDefensePower, 10f, 10, null);
        unitList = new List<Unit>();

        hpText.text = Hp.ToString();
    }

    void Start()
    {
        InitTargetBarracks();
    }

    private void InitTargetBarracks()
    {
        TargetBarracks = battleManager.GetNextTargetBarracks(this);
    }

    void Update()
    {
        if (TargetBarracks)
        {
            timeStack += Time.deltaTime;

            if (timeStack >= producePeriod)
            {
                ProduceUnit();
                timeStack = 0f;
            }

            if (TargetBarracks.Hp <= 0)
            {
                ChangeTargetBarracks(battleManager.GetNextTargetBarracks(this));
            }
        }
    }

    [Button(Name = "Produce Unit")]
    private void ProduceUnit()
    {
        var unit = Instantiate(unitPrefab, transform.position, new Quaternion(), battleArea).GetComponent<Unit>();
        //var unit = Instantiate(unitPrefab, transform).GetComponent<Unit>();

        unit.Spec = new UnitSpec(
            ProducingUnitSpec.BelongedFaction,
            ProducingUnitSpec.Hp,
            ProducingUnitSpec.AttackPower,
            ProducingUnitSpec.DefensePower,
            ProducingUnitSpec.Speed,
            ProducingUnitSpec.UnitSize,
            ProducingUnitSpec.DamagedImageList
        );
        unit.BelongedBarracks = this;
        unit.TargetBarracks = TargetBarracks;
        unit.hpText.text = unit.Spec.Hp.ToString();

        unitList.Add(unit);

        //unit.MoveTo(TargetBarracksPosition.anchoredPosition);
        unit.MoveToTarget();
    }

    public void RemoveUnitFromList(Unit unit)
    {
        unitList.Remove(unit);
    }

    public void OnDamaged(int attackPower)
    {
        Hp -= attackPower;
        hpText.text = Hp.ToString();

        if (Hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void ChangeTargetBarracks(Barracks targetBarracks)
    {
        TargetBarracks = targetBarracks;

        foreach (Unit unit in unitList)
        {
            unit.TargetBarracks = TargetBarracks;
        }
    }
}
