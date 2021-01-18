using System.Collections;
using System.Collections.Generic;

public class Turn
{
    public readonly List<Resource> startResources;
    public List<Resource> resultResources;
    public readonly List<ActionBundleElement> actionBundle;
    public int LockIndex { get; set; }
    public TurnStatus Status { get; set; }
    public float Period { get; set; }
    public float PlayTime { get; set; }


    public Turn(List<Resource> resources, List<ActionBundleElement> actionBundle, int lockIndex, float period)
    {
        startResources = resources.ConvertAll(res => new Resource(res.resourceType, res.Storage));
        resultResources = resources.ConvertAll(res => new Resource(res.resourceType, res.Storage));
        this.actionBundle = actionBundle;
        LockIndex = lockIndex;
        Status = TurnStatus.Wait;
        Period = period;
        PlayTime = 0f;
    }

    public bool IsEnoughPlayTime(int nowActionIndex)
    {
        return PlayTime >= Period / LockIndex * (nowActionIndex + 1);
    }
}
