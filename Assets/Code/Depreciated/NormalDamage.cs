using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Damage")]
public class NormalDamage : Effect
{
    public override bool IsDamage => true;
    /*
    public override void ApplyEffect(Unit actor, Unit target, ref AbilityResultList results)
    {
        AbilityResult r = new AbilityResult()
        {
            target = target,
            amount = (int)((float) actor.stats.Damage() * dmgmod),
            result = Result.hit
        };
        results.targets.Add(r);
        target.stats.TakeDamage(r.amount);
    }
    */
}
