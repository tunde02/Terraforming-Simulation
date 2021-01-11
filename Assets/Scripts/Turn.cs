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


    public Turn(List<Resource> resources, List<ActionType> actionBundle, float period)
    {
        startResources = resources.ConvertAll(res => new Resource(res.resourceType, res.Storage));
        endResources = resources.ConvertAll(res => new Resource(res.resourceType, res.Storage));
        this.actionBundle = actionBundle;
        Status = TurnStatus.Wait;
        Period = period;
        PlayTime = 0f;
    }

    public bool IsEnoughPlayTime(int nowActionIndex)
    {
        return PlayTime >= Period / actionBundle.Count * (nowActionIndex + 1);
    }
}
