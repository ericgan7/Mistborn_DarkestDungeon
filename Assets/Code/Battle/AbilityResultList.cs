using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityResultList
{
    public Unit actor;
    public Ability ability;
    public List<AbilityResult> targets;
    public List<AbilityResult> counter;

    public bool display;
    public List<DelayedAbilityResult> delayedEffects;

    public AbilityResultList()
    {
        targets = new List<AbilityResult>();
        delayedEffects = new List<DelayedAbilityResult>();
        counter = new List<AbilityResult>();
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

    public void Display(float delay = 0f)
    {
        if (delayedEffect.Type == EffectType.none){
            return;
        }
        if (result == DelayedResult.resist)
        {
            target.CreatePopUpText("Resisted", ColorPallete.GetEffectColor(delayedEffect.Type), delay);
        } else
        {
            target.CreatePopUpText(effectNames[delayedEffect.Type], ColorPallete.GetEffectColor(delayedEffect.Type), delay);
        }
    }

    public void Apply(Unit actor)
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

    public void Display(float delay = 0f)
    {
        switch (result)
        {
            case Result.miss:
                target.CreatePopUpText("Miss", ColorPallete.GetResultColor(result));
                break;
            case Result.buff:
                target.CreatePopUpText("Buff", ColorPallete.GetResultColor(result));
                break;
            default:
                target.CreatePopUpText(amount.ToString(), ColorPallete.GetResultColor(result));
                break;
        }
        ApplyEffects();
    }

    public void ApplyEffects()
    {
        switch (result)
        {
            case Result.def:
                target.stats.GainArmor(amount);
                break;
            case Result.heal:
                target.stats.Heal(amount);
                break;
            default:
                target.stats.TakeDamage(amount);
                break;
        }
        target.UpdateUI();
    }
    //need enough to determine animation
}
