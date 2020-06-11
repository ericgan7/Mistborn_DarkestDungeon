using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Formation")]
public class Formation : ScriptableObject
{
    public bool[] ranks;
    public Sprite single;
    public Sprite aoe;
    
    public virtual bool IsValidRank(Unit actor, Unit target, bool isCombat = true)
    {
        return ranks[target.Location];
    }
}


