using System.Collections;
using System.Collections.Generic;

public class Turn
{
    public readonly List<Resource> startResources;
    public List<Resource> endResources;
    public readonly List<ActionType> actionBundle;
    public TurnStatus Status { get; set; }
    public float Period { get; set; }
    public float PlayTime { get; set; }


    public Turn(List<Resource> startResources, List<ActionType> actionBundle, float period)
    {
        this.startResources = new List<Resource>(startResources);
        endResources = new List<Resource>(startResources);
        this.actionBundle = new List<ActionType>(actionBundle);
        Status = TurnStatus.Wait;
        Period = period;
        PlayTime = 0f;
    }

    public bool IsEnoughPlayTime(int nowActionIndex)
    {
        return PlayTime >= Period / actionBundle.Count * (nowActionIndex + 1);
    }
}
