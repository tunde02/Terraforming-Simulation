using System.Collections;
using System.Collections.Generic;


public enum TurnStatus { WAITING, PLAYING, PAUSED }

public class Turn
{
    public delegate void StatusHandler(Turn turn, TurnStatus prevStatus);
    public delegate void ScenarioHandler(Turn turn);

    public static event StatusHandler OnTurnStatusChanged;
    public static event ScenarioHandler OnScenarioChanged;


    private TurnStatus status = TurnStatus.WAITING;
    public TurnStatus Status
    {
        get
        {
            return status;
        }
        set
        {
            TurnStatus prevStatus = status;

            status = value;

            OnTurnStatusChanged(this, prevStatus);
        }
    }
    public List<Resource> StartingResources { get; private set; }
    public List<Resource> ResultResources { get; set; }
    public List<ActionSlot> Scenario { get; private set; }
    public int LockedIndex { get; private set; }
    public float Period { get; set; }
    private float playtime = 0f;
    public float PlayTime
    {
        get
        {
            return playtime;
        }
        set
        {
            playtime = value;

            if (IsEnoughPlayTime())
                Scenario[NowSlotIndex++].PlacedAction.PerformAction(ResultResources);
        }
    }
    private int nowSlotIndex = 0;
    public int NowSlotIndex
    {
        get
        {
            return nowSlotIndex;
        }
        set
        {
            nowSlotIndex = value;

            if (nowSlotIndex >= LockedIndex)
            {
                status = TurnStatus.WAITING;
                OnTurnStatusChanged(this, TurnStatus.PLAYING);
            }
        }
    }


    public Turn(List<Resource> resources, List<ActionSlot> scenario, int lockedIndex, float period)
    {
        StartingResources = resources.ConvertAll(resource => new Resource(resource.Type, resource.Storage));
        ResultResources = resources;
        Scenario = scenario.ConvertAll(slot => new ActionSlot(slot.PlacedAction, slot.IsEmpty, slot.IsLocked));
        LockedIndex = lockedIndex;
        Period = period;

        OnScenarioChanged(this);
    }

    private bool IsEnoughPlayTime()
    {
        return PlayTime >= Period / LockedIndex * (nowSlotIndex + 1);
    }

    public bool IsEnoughPlayTime(int nowSlotIndex)
    {
        return PlayTime >= Period / LockedIndex * (nowSlotIndex + 1);
    }
}
