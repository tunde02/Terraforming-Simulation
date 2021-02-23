using System.Collections;
using System.Collections.Generic;


public enum TurnStatus { WAITING, PLAYING, PAUSED }

public class Turn
{
    public delegate void TurnStatusHandler();
    public delegate void ScenarioHandler(int lockedIndex, List<ActionSlot> scenario);

    public static event TurnStatusHandler OnTurnStarted;
    public static event TurnStatusHandler OnTurnPaused;
    public static event TurnStatusHandler OnTurnResumed;
    public static event TurnStatusHandler OnTurnFinished;
    public static event ScenarioHandler OnScenarioChanged;


    private TurnStatus status;
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

            if (prevStatus == TurnStatus.WAITING && status == TurnStatus.PLAYING)
            {
                OnTurnStarted();
            }
            else if (prevStatus == TurnStatus.PLAYING && status == TurnStatus.PAUSED)
            {
                OnTurnPaused();
            }
            else if (prevStatus == TurnStatus.PAUSED && status == TurnStatus.PLAYING)
            {
                OnTurnResumed();
            }
            else if (prevStatus == TurnStatus.PLAYING && status == TurnStatus.WAITING)
            {
                OnTurnFinished();
            }
            else
            {
                // error
            }
        }
    }
    public List<Resource> StartingResources { get; private set; }
    public List<Resource> ResultResources { get; set; }
    public List<ActionSlot> Scenario { get; private set; }
    public int LockedIndex { get; private set; }
    public float Period { get; private set; }
    private float playtime;
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
                Scenario[NowSlotIndex].PlacedAction.PerformAction(ResultResources, Scenario[NowSlotIndex++].BlockWeight);
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
                OnTurnFinished();
            }
        }
    }


    public Turn(TurnStatus status, List<Resource> resources, List<ActionSlot> scenario, int lockedIndex, float period)
    {
        this.status = status;
        StartingResources = resources.ConvertAll(resource => new Resource(resource.Type, resource.Storage));
        ResultResources = resources;
        Scenario = scenario.ConvertAll(slot => new ActionSlot(slot.PlacedAction, slot.IsEmpty, slot.IsLocked, slot.BlockWeight));
        LockedIndex = lockedIndex;
        Period = period;
        PlayTime = 0f;

        OnScenarioChanged(LockedIndex, Scenario);
    }

    private bool IsEnoughPlayTime()
    {
        return PlayTime >= Period / LockedIndex * (nowSlotIndex + 1);
    }
}
