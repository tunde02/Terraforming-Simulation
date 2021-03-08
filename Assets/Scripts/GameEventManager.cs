using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class GameEventManager : BaseManager
{
    public List<GameEvent> EventList { get; set; }
    private ActionManager actionManager;
    private TurnManager turnManager;
    private int seasonNumber = 1;


    [Inject]
    public void Construct(ActionManager actionManager, TurnManager turnManager)
    {
        this.actionManager = actionManager;
        this.turnManager = turnManager;
    }

    public override void Initialize()
    {
        EventList = new List<GameEvent> {
            new GameEvent(GameEventType.Spring)
        };
        //EventList = new List<GameEvent>();

        Turn.OnTurnFinished += ProceedGameEvent;
        Turn.OnTurnFinished += GenerateGameEvent;
        Turn.OnTurnFinished += PrintEventList;
    }

    private void PrintEventList()
    {
        string str = $"NOW EVENT LIST ( TURN: {turnManager.TurnNumber} ) : ";
        foreach (var e in EventList)
        {
            str += $"{e.EventType}({e.RemainingTurn}), ";
        }

        Debug.Log(str);
    }

    private void GenerateGameEvent()
    {
        if (turnManager.TurnNumber == 0) return;

        System.Random r = new System.Random();
        int turnNumber = turnManager.TurnNumber;

        // Season
        if (turnNumber % 5 == 0)
        {
            switch (seasonNumber)
            {
                case 0:
                    EventList.Add(new GameEvent(GameEventType.Spring));
                    break;
                case 1:
                    EventList.Add(new GameEvent(GameEventType.Summer));
                    break;
                case 2:
                    EventList.Add(new GameEvent(GameEventType.Autumn));
                    break;
                case 3:
                    EventList.Add(new GameEvent(GameEventType.Winter));
                    break;
            }

            seasonNumber = (seasonNumber + 1) % 4;
        }

        // NaturalDisasters
        if (turnNumber % 10 == 0 && r.Next(0, 10) == 0) // 10%
        {
            EventList.Add(new GameEvent(GameEventType.NaturalDisasters));
        }

        // GoldenAge
        // 자원비율 확인을 어떻게 할까?

        // GenerationChange
        if (turnNumber % 20 == 0)
        {
            EventList.Add(new GameEvent(GameEventType.GenerationChange));

            // ExtraordinaryLeader
            if (r.Next(0, 10) <= 2) // 30%
            {
                EventList.Add(new GameEvent(GameEventType.ExtraordinaryLeader));
            }
        }

        // War

        // MilitaryDodge
    }

    private void ProceedGameEvent()
    {
        List<GameEvent> endEventList = new List<GameEvent>();

        foreach (var e in EventList)
        {
            if (--e.RemainingTurn <= 0)
            {
                EndGameEvent(e);
                endEventList.Add(e);
            }
        }

        foreach (var endEvent in endEventList)
        {
            EventList.Remove(endEvent);
        }
    }

    private void StartGameEvent(GameEvent targetEvent)
    {
        // TODO:
        actionManager.ACTION[0].Weights.Add(targetEvent.ActionWeights[0]);
        actionManager.ACTION[1].Weights.Add(targetEvent.ActionWeights[1]);
        actionManager.ACTION[2].Weights.Add(targetEvent.ActionWeights[2]);
        actionManager.ACTION[3].Weights.Add(targetEvent.ActionWeights[3]);
    }

    private void EndGameEvent(GameEvent targetEvent)
    {
        actionManager.ACTION[0].Weights.Remove(targetEvent.ActionWeights[0]);
        actionManager.ACTION[1].Weights.Remove(targetEvent.ActionWeights[1]);
        actionManager.ACTION[2].Weights.Remove(targetEvent.ActionWeights[2]);
        actionManager.ACTION[3].Weights.Remove(targetEvent.ActionWeights[3]);
    }
}
