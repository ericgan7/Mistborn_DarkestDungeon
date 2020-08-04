using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Counter : StatusEffect
{
    float multiplier;
    public override bool IsCounter => true;

    public Counter(float m, int d)
    {
        multiplier = m;
        duration = d;
    }
    public override void Counterattack(Unit actor, Unit target, ref AbilityResultList results)
    {
        //TODO roll for miss
        AbilityResult r = new AbilityResult()
        {
            target = target,
            amount = (int)(target.stats.GetStat(StatType.damage) * multiplier),
            result = Result.Hit
        };
        actor.stats.TakeDamage(r.amount);
        results.counter.Add(r);
    }

    public override string ToString()
    {
        return string.Format("Activates Counter for {0} turns", duration);
    }
}
