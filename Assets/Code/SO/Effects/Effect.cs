using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Effect
{
    //boolean access for ui;
    public virtual bool IsHeal { get { return false; } }
    public virtual bool IsDamage { get { return false; } }
    public virtual bool IsMove { get { return false; } }



    //apply damage, dot, buffs, etc.
    public virtual void ApplyEffect(Unit actor, Unit target, ref AbilityResultList results)
    {
        DelayedAbilityResult r = new DelayedAbilityResult()
        {
            target = target,
            delayedEffect = this
        };
        results.delayedEffects.Add(r);
    }

    public virtual void ApplyDelayedEffect(Unit actor, Unit target) { }

    public virtual string Stats() { return ""; }
}
