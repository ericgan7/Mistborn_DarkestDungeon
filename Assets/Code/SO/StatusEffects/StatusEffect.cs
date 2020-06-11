using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType{
    None,
    IsMarked
}

public abstract class StatusEffect : Effect
{
    protected int duration;
    
    public virtual bool IsPermanant { get { return false; } }
    public virtual bool IsStatChange { get { return false; } }
    public virtual bool IsCounter { get { return false; } }
    public virtual bool IsMark { get { return false; } }
    public virtual bool IsDebuff { get { return false; } }

    public override void ApplyDelayedEffect(Unit actor, Unit target)
    {
        target.stats.ApplyDelayedEffect(this);
    }

    public virtual bool DecreaseDuration()
    {
        duration -= 1;
        if (duration <= 0)
        {
            return true;
        }
        return false;
    }

    public virtual void Counterattack(Unit actor, Unit Target, ref AbilityResultList results) { }

    public virtual int ApplyOverTime(Unit target) { return 0; } //duration effects

    public virtual float GetStat(StatType type, Unit actor = null, Unit target = null) { return 0f; }
}

public abstract class Traits : StatusEffect
{
    public override bool IsPermanant => true;
    public override EffectType Type => EffectType.baseClass;

    public override bool DecreaseDuration()
    {
        return false;
    }

    public virtual void OnAttack(Unit actor, Unit Target) {}

    public virtual void OnDefend(Unit actor, int amount) {}

    public virtual void OnMove(Unit actor) {}

    public virtual void OnCrit(){}

    public virtual void ModifyAttack(Ability_Attack a, Unit actor, Unit target, ref AbilityResultList results){}
}