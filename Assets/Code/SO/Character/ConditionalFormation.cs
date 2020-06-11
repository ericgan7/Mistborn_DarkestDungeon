using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Conditional Formation")]
public class ConditionalFormation :Formation
{
    [SerializeField] int amount;
    public override bool IsValidRank(Unit actor, Unit target, bool isCombat = true)
    {
        if (isCombat){
            int distance = Mathf.Abs(target.Location - actor.Location);
            int movement = Mathf.Max(actor.stats.GetStat(StatType.speed), 1);
            return distance > 0 && distance <= movement ;
        } else {
            return true;
        }
    }
}
