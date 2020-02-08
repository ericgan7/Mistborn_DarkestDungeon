using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StressHeal : Effect
{
    int amount;
    public StressHeal(int a)
    {
        amount = a;
    }
    public override void ApplyEffect(Unit actor, Unit target, ref AbilityResultList results)
    {
        AbilityResult r = new AbilityResult()
        {
            target = target,
            amount = amount,
            result = Result.heal
        };
        results.targets.Add(r);
        target.stats.Heal(amount);
    }
}
