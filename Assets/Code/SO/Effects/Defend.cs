using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Defend : Effect
{
    int amount;
    public Defend(int a)
    {
        amount = a;
    }

    public override void ApplyEffect(Unit actor, Unit target, ref AbilityResultList results)
    {
        //TODO: roll for crit def
        AbilityResult r = new AbilityResult()
        {
            target = target,
            amount = amount,
            result = Result.def
        };
        results.targets.Add(r);
        target.stats.GainArmor(amount);
    }
}
