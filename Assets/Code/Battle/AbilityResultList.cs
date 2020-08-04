using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityResultList
{
    public Unit actor;
    public Ability ability;
    public List<AbilityResult> targets;
    public List<AbilityResult> counter;
    public List<DelayedAbilityResult> stress;

    public bool display;
    public List<DelayedAbilityResult> delayedEffects;

    public AbilityResultList()
    {
        targets = new List<AbilityResult>();
        delayedEffects = new List<DelayedAbilityResult>();
        counter = new List<AbilityResult>();
        stress = new List<DelayedAbilityResult>();
    }

}

public class DelayedAbilityResult {
    public Unit target;
    public Effect delayedEffect;
    public DelayedResult result;

    static readonly Dictionary<EffectType, string> effectNames = new Dictionary<EffectType, string>()
    {
        {EffectType.buff, "Buff"},
        {EffectType.debuff, "Debuff"},
        {EffectType.bleed, "Bleed"},
        {EffectType.stun, "Stun"},
        {EffectType.mark, "Mark"},
        {EffectType.block, "Block"},
        {EffectType.move, "Move"},
        {EffectType.guard, "Guard"},
        {EffectType.stress, "Stress"}
    };

    public void Display()
    {
        if (delayedEffect.Type == EffectType.none){
            return;
        }
        if (result == DelayedResult.resist)
        {
            target.CreatePopUpText("Resisted", ColorPallete.GetEffectColor(delayedEffect.Type));
        } else if (delayedEffect.Type == EffectType.stress){
            target.CreatePopUpText(string.Format("{0}: {1}", 
                effectNames[delayedEffect.Type], ((Stress) delayedEffect).StressDamage(target)), ColorPallete.GetEffectColor(EffectType.stress));
        } else 
        {
            target.CreatePopUpText(effectNames[delayedEffect.Type], ColorPallete.GetEffectColor(delayedEffect.Type));
        }
    }

    public void ApplyEffect(Unit actor)
    {
        if (result == DelayedResult.hit)
        {
            delayedEffect.ApplyDelayedEffect(actor, target);
        }
    }
}

public class AbilityResult {
    public Unit actor;
    public Unit target;
    public int amount;
    public Result result;

    public static AbilityResult None = new AbilityResult(){ target = null, amount = 0, result = Result.none};

    public void Display()
    {
        switch (result)
        {
            case Result.none:
                break;
            case Result.Hit:
            case Result.Graze:
            case Result.Crit:
                target.CreatePopUpText(amount.ToString(), ColorPallete.GetResultColor(result));
                break;
            default:
                target.CreatePopUpText(result.ToString(), ColorPallete.GetResultColor(result));
                break;
        }
        ApplyEffects();
    }

    public int ApplyEffects()
    {
        switch (result)
        {
            case Result.Def:
                target.stats.GainArmor(amount);
                break;
            case Result.Heal:
                target.stats.Heal(amount);
                break;
            default:
                target.stats.TakeDamage(amount);
                break;
        }
        target.UpdateUI();
        return amount;
    }
    //need enough to determine animation
}
