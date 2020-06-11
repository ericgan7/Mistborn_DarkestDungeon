using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stress : Effect
{
    int stressAmount;
    public Stress(int amount){
        stressAmount = amount;
    }

    public override void ApplyEffect(Unit actor, Unit target, ref AbilityResultList results)
    {
        AbilityResult r = new AbilityResult()
        {
            actor = actor,
            target = target,
            amount = stressAmount,
            result = stressAmount < 0 ? Result.stress : Result.stressheal
        };
        results.targets.Add(r);
        target.stats.StressDamage(stressAmount);
    }

    public int StressDamage(Unit target){
        return stressAmount - target.stats.GetStat(StatType.stressResist) ;
    }

    public override string ToString(Unit target){
        int amount = StressDamage(target);
        return string.Format("{0} {1} <color={2}></b>Stress<b></color>\n", 
            amount >= 0 ? "Deal" : "Recover", amount, ColorPallete.GetHexColor("Purple"));
    }
}
