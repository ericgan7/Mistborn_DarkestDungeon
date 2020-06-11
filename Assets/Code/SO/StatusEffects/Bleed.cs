using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleed : StatusEffect
{
    int amount;
    public override EffectType Type => EffectType.bleed;
    public Bleed(int a, int d)
    {
        amount = a;
        duration = d;
    }

    public override int ApplyOverTime(Unit target)
    {
        target.stats.TakeDamage(amount);
        return amount;
    }

    public override string ToString()
    {
        return string.Format("<color={2}><b>Bleed</b></color> {0} dmg for {1} rds\n", 
            amount, duration, ColorPallete.GetHexColor("Highlight Red"));
    }

    public override string ToString(Unit target){
        return string.Format("<color={1}><b>Bleed</b></color>: {0}% chance\n", 
            100 - target.stats.GetStat(StatType.bleedResist), ColorPallete.GetHexColor("Highlight Red"));
    }
}
