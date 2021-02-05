using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum Faction { HUMAN, ALIEN, EXTERRAINS }

public class UnitSpec
{
    public Faction BelongedFaction { get; set; }
    public int Hp { get; set; }
    public int AttackPower;
    public int DefensePower { get; set; }
    public float Speed { get; set; }
    public int UnitSize { get; set; }
    public Barracks TargetBarracks { get; set; }
    public List<Image> DamagedImageList { get; set; }


    public UnitSpec()
    {
        BelongedFaction = Faction.HUMAN;
        Hp = 100;
        AttackPower = 10;
        DefensePower = 2;
        Speed = 5f;
        UnitSize = 10;
        TargetBarracks = null;
        DamagedImageList = null;
    }

    public UnitSpec(Faction belongedFaction, int hp, int attackPower, int defensePower, float speed, int unitSize, Barracks targetBarracks, List<Image> damagedImageList)
    {
        BelongedFaction = belongedFaction;
        Hp = hp;
        AttackPower = attackPower;
        DefensePower = defensePower;
        Speed = speed;
        UnitSize = unitSize;
        TargetBarracks = targetBarracks;
        DamagedImageList = damagedImageList;
    }
}
