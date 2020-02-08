using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleed : StatusEffect
{
    int amount;

    public Bleed(int d, int a)
    {
        duration = d;
        amount = a;
    }
    public override void ApplyDelayedEffect(Unit actor, Unit target)
    {
        target.stats.modifiers.Add(this);
    }

    public override void ApplyOverTime(Unit target)
    {
        target.stats.TakeDamage(amount);
        if (DecreaseDuration())
        {
            target.stats.RemoveEffect(this);
        }
    }

    public override string Stats()
    {
        string result = string.Format("Bleed {0} dmg for {1} rds\n", amount, duration);
        return result;
    }
}
