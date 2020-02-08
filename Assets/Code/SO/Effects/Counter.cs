using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Counter : StatusEffect
{
    float multiplier;
    public override bool IsCounter => true;

    public Counter(int d, float m)
    {
        duration = d;
        multiplier = m;
    }
    public override void Counterattack(Unit actor, Unit target, ref AbilityResultList results)
    {
        //TODO roll for miss
        AbilityResult r = new AbilityResult()
        {
            target = target,
            amount = (int)(target.stats.Damage() * multiplier),
            result = Result.hit
        };
        actor.stats.TakeDamage(r.amount);
        results.counter.Add(r);
    }

    public override string Stats()
    {
        return string.Format("Activates Counter for {0} turns", duration);
    }
}
