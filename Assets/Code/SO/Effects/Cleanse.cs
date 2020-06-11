using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleanse : Effect
{
    public override void ApplyDelayedEffect(Unit actor, Unit target)
    {
        target.stats.modifiers.ClearAllEffects();
    }

    public override string ToString(Unit target){
        return string.Format("Clear all <color={0}><b>debuffs</b></color>\n",
        ColorPallete.GetHexColor("Orange"));
    } 
}
