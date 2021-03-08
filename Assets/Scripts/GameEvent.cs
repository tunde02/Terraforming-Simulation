using System.Collections;
using System.Collections.Generic;
using System;


public enum GameEventType {
    Spring, Summer, Autumn, Winter, NaturalDisasters, GoldenAge, GenerationChange, ExtraordinaryLeader, War, MilitaryDodge
}

public class GameEvent
{
    public GameEventType EventType { get; private set; }
    public int RemainingTurn { get; set; }
    public List<long> Rewards { get; private set; }
    public List<double> ActionWeights { get; private set; }
    //public Player TargetPlayer { get; private set; }

    
    public GameEvent(GameEventType gameEventType)
    {
        Random r = new Random();
        EventType = gameEventType;
        
        switch (EventType)
        {
            case GameEventType.Spring:
                RemainingTurn = 5;
                Rewards = new List<long> { 0, 0, 0, 0 };
                ActionWeights = new List<double> { 1.1, 1.1, 1.1, 1.1 };
                break;
            case GameEventType.Summer:
                RemainingTurn = 5;
                Rewards = new List<long> { 0, 0, 0, 0 };
                ActionWeights = new List<double> { 1.2, 1.0, 1.0, 0.95 };
                break;
            case GameEventType.Autumn:
                RemainingTurn = 5;
                Rewards = new List<long> { 0, 0, 0, 0 };
                ActionWeights = new List<double> { 1.0, 1.25, 1.0, 1.0 };
                break;
            case GameEventType.Winter:
                RemainingTurn = 5;
                Rewards = new List<long> { 0, 0, 0, 0 };
                ActionWeights = new List<double> { 0.9, 0.9, 0.9, 0.9 };
                break;
            case GameEventType.NaturalDisasters:
                RemainingTurn = 5;
                Rewards = new List<long> { 0, 0, 0, 0 };
                ActionWeights = new List<double> { 1.0, 1.0, 1.0, 1.0 };
                ActionWeights[r.Next(0, 4)] = 0.8;
                break;
            case GameEventType.GoldenAge:
                RemainingTurn = 5;
                Rewards = new List<long> { 0, 0, 0, 0 };
                ActionWeights = new List<double> { 1.3, 1.3, 1.3, 1.3 };
                break;
            case GameEventType.GenerationChange:
                RemainingTurn = 5;
                Rewards = new List<long> { 0, 0, 0, 0 };
                if (r.Next(0, 2) == 0)
                    ActionWeights = new List<double> { 1.0, 1.0, 1.0, 1.2 };
                else
                    ActionWeights = new List<double> { 1.0, 1.0, 1.0, 0.8 };
                break;
            case GameEventType.ExtraordinaryLeader:
                break;
            case GameEventType.War:
                break;
            case GameEventType.MilitaryDodge:
                RemainingTurn = 3;
                Rewards = new List<long> { 0, 0, 0, 0 };
                ActionWeights = new List<double> { 1.0, 1.0, 1.0, 0.8 };
                break;
        }
    }

    public string GetExplainText()
    {
        string explainText = "";

        switch (EventType)
        {
            case GameEventType.Spring:
                break;
            case GameEventType.Summer:
                break;
            case GameEventType.Autumn:
                break;
            case GameEventType.Winter:
                break;
            case GameEventType.NaturalDisasters:
                break;
            case GameEventType.GoldenAge:
                break;
            case GameEventType.GenerationChange:
                break;
            case GameEventType.ExtraordinaryLeader:
                break;
            case GameEventType.War:
                break;
            case GameEventType.MilitaryDodge:
                break;
        }

        return explainText;
    }
}
