using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class BattleManager : MonoBehaviour
{
    public List<Barracks> humanBarracksGroup;
    public List<Barracks> alienBarracksGroup;
    public List<Barracks> exterrainsBarracksGroup;


    public Battle NowBattle { get; set; }
    private TurnManager turnManager;


    [Inject]
    public void Construct(TurnManager turnManager)
    {
        this.turnManager = turnManager;
    }

    void Awake()
    {
        NowBattle = new Battle(Faction.HUMAN, Faction.EXTERRAINS, null);
    }

    void Update()
    {

    }

    private void InitTargetBarracks()
    {
         
    }

    public Barracks GetNextTargetBarracks(Barracks barracks)
    {
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

        foreach (Barracks b in enemyBarracksGroup)
        {
            if (b.Hp > 0)
            {
                return b;
            }
        }

        Debug.Log("No More Enemy Barracks!!");
        return null;
    }
}
