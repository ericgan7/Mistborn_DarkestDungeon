using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityResultList
{
    public Unit actor;
    public Ability ability;
    public List<AbilityResult> targets;
    public List<AbilityResult> counter;

    public List<DelayedAbilityResult> delayedEffects;

    public AbilityResultList()
    {
        targets = new List<AbilityResult>();
        delayedEffects = new List<DelayedAbilityResult>();
    }

}

public class DelayedAbilityResult {
    public Unit target;
    public Effect delayedEffect;

    public void Display()
    {

    }

    public void Apply(Unit actor)
    {
        delayedEffect.ApplyDelayedEffect(actor, target);
    }
}

public class AbilityResult {
    static readonly Dictionary<Result, Color> colors = new Dictionary<Result, Color>()
    {
        { Result.hit, Color.red },
        { Result.miss, Color.red },
        { Result.graze, Color.red },
        { Result.heal, Color.green },
        { Result.def, Color.blue },
        { Result.buff, Color.blue },
        { Result.crit, Color.yellow },
        { Result.defcrit, Color.cyan }
    };

    public Unit target;
    public int amount;
    public Result result;

    public void Display()
    {
        switch (result)
        {
            case Result.miss:
                target.SpawnText("Miss", colors[result]);
                break;
            case Result.buff:
                target.SpawnText("Buff", colors[result]);
                break;
            default:
                target.SpawnText(amount.ToString(), colors[result]);
                break;
        }
    }
    //need enough to determine animation
}
