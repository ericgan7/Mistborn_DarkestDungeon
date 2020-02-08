using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Formation")]
public class Formation : ScriptableObject
{
    public bool[] ranks;
    public Sprite single;
    public Sprite aoe;
    
    public virtual bool IsValidRank(Unit actor, Unit target)
    {
        return ranks[target.Location];
    }
}

[CreateAssetMenu(menuName = "Conditional Formation")]
public class ConditionalFormation :Formation
{
    public int amount;
    public override bool IsValidRank(Unit actor, Unit target)
    {
        Debug.Log(actor.Location);
        Debug.Log(target.Location);
        int distance = Mathf.Abs(target.Location - actor.Location);
        return distance > 0 && distance <= amount ;
    }
}
