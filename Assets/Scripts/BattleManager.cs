using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public enum Tactic { LINEAR, RANDOM, HIGHEST_HP, LOWEST_HP, STRONGEST, WEAKEST }

public class BattleManager : BaseManager
{
    public List<Barracks> humanBarracksGroup;
    public List<Barracks> alienBarracksGroup;
    public List<Barracks> exterrainsBarracksGroup;
    public Tactic tactic;


    public Battle NowBattle { get; set; }
    private GameManager gameManager;
    private TurnManager turnManager;


    [Inject]
    public void Construct(GameManager gameManager, TurnManager turnManager)
    {
        this.gameManager = gameManager;
        this.turnManager = turnManager;
    }

    public override void Initialize()
    {
        NowBattle = new Battle(Faction.HUMAN, Faction.EXTERRAINS, null);
        //tactic = Tactic.RANDOM;
        
        InitTargetBarracks();
    }

    private void InitTargetBarracks()
    {
        foreach (Barracks b in humanBarracksGroup)
        {
            b.TargetBarracks = GetNextTargetBarracks(b);
        }

        foreach (Barracks b in exterrainsBarracksGroup)
        {
            b.TargetBarracks = GetNextTargetBarracks(b);
        }
    }

    public Barracks GetNextTargetBarracks(Barracks barracks)
    {
        Barracks nextTargetBarracks = null;
        Faction oppositeFaction = barracks.BelongedFaction == NowBattle.Faction1 ? NowBattle.Faction2 : NowBattle.Faction1;
        List<Barracks> enemyBarracksGroup;

        switch (oppositeFaction)
        {
            case Faction.HUMAN:
                enemyBarracksGroup = humanBarracksGroup;
                break;
            //case Faction.ALIEN:
            //    enemyBarracksGroup = alienBarracksGroup;
            //    break;
            case Faction.EXTERRAINS:
                enemyBarracksGroup = exterrainsBarracksGroup;
                break;
            default:
                Debug.LogError("Invalid NowBattle Faction : BattleManager.cs - ChangeTargetBarracks()");
                return null;
        }

        switch (tactic)
        {
            case Tactic.LINEAR:
                foreach (Barracks b in enemyBarracksGroup)
                {
                    if (b.Hp > 0)
                    {
                        nextTargetBarracks = b;
                        break;
                    }
                }
                break;
            case Tactic.RANDOM:
                List<Barracks> tempList = new List<Barracks>();

                foreach (Barracks b in enemyBarracksGroup)
                {
                    if (b.Hp > 0)
                    {
                        tempList.Add(b);
                    }
                }

                if (tempList.Count > 0)
                {
                    nextTargetBarracks = tempList[Random.Range(0, tempList.Count)];
                }
                break;
            case Tactic.HIGHEST_HP:
                Barracks highestHpBarracks = null;
                int highestHp = 0;

                foreach (Barracks b in enemyBarracksGroup)
                {
                    if (highestHp < b.Hp)
                    {
                        highestHpBarracks = b;
                        highestHp = b.Hp;
                    }
                }

                nextTargetBarracks = highestHpBarracks;
                break;
            case Tactic.LOWEST_HP:
                Barracks lowestHpBarracks = null;
                int lowestHp = 9999999;

                foreach (Barracks b in enemyBarracksGroup)
                {
                    if (b.Hp > 0 && lowestHp > b.Hp)
                    {
                        lowestHpBarracks = b;
                        lowestHp = b.Hp;
                    }
                }

                nextTargetBarracks = lowestHpBarracks;
                break;
            case Tactic.STRONGEST:
                Barracks strongestUnitBarracks = null;
                int strongestAttackPower = 0;

                foreach (Barracks b in enemyBarracksGroup)
                {
                    if (b.Hp > 0 && strongestAttackPower < b.ProducingUnitSpec.AttackPower)
                    {
                        strongestUnitBarracks = b;
                        strongestAttackPower = b.ProducingUnitSpec.AttackPower;
                    }
                }

                nextTargetBarracks = strongestUnitBarracks;
                break;
            case Tactic.WEAKEST:
                Barracks weakestUnitBarracks = null;
                int weakestAttackPower = 9999999;

                foreach (Barracks b in enemyBarracksGroup)
                {
                    if (b.Hp > 0 && weakestAttackPower > b.ProducingUnitSpec.AttackPower)
                    {
                        weakestUnitBarracks = b;
                        weakestAttackPower = b.ProducingUnitSpec.AttackPower;
                    }
                }

                nextTargetBarracks = weakestUnitBarracks;
                break;
        }

        if (!nextTargetBarracks)
        {
            Debug.Log("No More Enemy Barracks!!");
        }

        return nextTargetBarracks;
    }
}
