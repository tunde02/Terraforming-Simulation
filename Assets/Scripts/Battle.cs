using System.Collections;
using System.Collections.Generic;


public class Battle
{
    public Faction Faction1 { get; set; }
    public Faction Faction2 { get; set; }
    public List<long> Reward { get; set; }


    public Battle(Faction faction1, Faction faction2, List<long> reward)
    {
        Faction1 = faction1;
        Faction2 = faction2;
        Reward = reward;
    }
}
