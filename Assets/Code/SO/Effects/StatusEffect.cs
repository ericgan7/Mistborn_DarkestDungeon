using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect : Effect
{
    protected int duration;
    //TODO icon?
    public virtual bool IsStatChange { get { return false; } }
    public virtual bool IsCounter { get { return false; } }
    public virtual bool IsMark { get { return false; } }

    public override void ApplyEffect(Unit actor, Unit target, ref AbilityResultList results)
    {
        DelayedAbilityResult r = new DelayedAbilityResult()
        {
            target = target,
            delayedEffect = this
        };
        results.delayedEffects.Add(r);
    }
    public override void ApplyDelayedEffect(Unit actor, Unit target)
    {
        target.stats.ApplyDelayedEffect(this);
    }

    public virtual bool DecreaseDuration()
    {
        duration -= 1;
        if (duration < 0)
        {
            return true;
        }
        return false;
    }

    public virtual void Counterattack(Unit actor, Unit Target, ref AbilityResultList results) {}

    public virtual void ApplyOverTime(Unit target) { } //duration effects

    public virtual float GetStat(StatType type) { return 0f; }
}
