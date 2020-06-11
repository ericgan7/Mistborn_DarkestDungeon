using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Effect
{
    //boolean access for ui;
    public virtual bool IsHeal { get { return false; } }
    public virtual bool IsDamage { get { return false; } }
    public virtual bool IsMove { get { return false; } }

    public virtual EffectType Type { get { return EffectType.none; } }
    protected static Dictionary<EffectType, StatType> ResistType =
        new Dictionary<EffectType, StatType>()
        {
            { EffectType.bleed, StatType.bleedResist},
            { EffectType.move, StatType.moveResist },
            { EffectType.debuff, StatType.debuffResist },
            { EffectType.stun, StatType.stunResist },
        };

    //apply damage, dot, buffs, etc.
    public virtual void ApplyEffect(Unit actor, Unit target, ref AbilityResultList results)
    {
        DelayedResult result = DelayedResult.hit;
        if (actor.UnitTeam.isAlly != target.UnitTeam.isAlly)
        {
            if (ResistType.ContainsKey(Type)) //resistable effect
            {
                int resist = target.stats.GetStat(ResistType[Type]);
                int roll = Random.Range(0, 100);
                if (roll < resist)
                {
                    result = DelayedResult.resist;
                }
            }
        }
        DelayedAbilityResult r = new DelayedAbilityResult()
        {
            target = target,
            delayedEffect = this,
            result = result,
        };
        results.delayedEffects.Add(r);
    }

    public virtual void ApplyDelayedEffect(Unit actor, Unit target) { }

    public virtual string ToString(Unit target) { return this.ToString(); }
}
