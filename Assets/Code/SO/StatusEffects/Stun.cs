using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : StatusEffect
{
    public override EffectType Type => EffectType.stun;
    public Stun()
    {
        duration = 3;
    }
    public override string ToString()
    {
        return string.Format("<color={0}><b>Stunned</b></color>: Skips next turn\n", ColorPallete.GetHexColor("Highlight Yellow"));
    }

    public override string ToString(Unit target){
        return string.Format("<color={1}><b>Stun</b></color>: {0}% chance\n", 
        100 - target.stats.GetStat(StatType.stunResist), ColorPallete.GetHexColor("Highlight Yellow"));
    }
}
