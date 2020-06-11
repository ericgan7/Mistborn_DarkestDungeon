using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Focus : Traits
{
    int stacks;
    int markAccBonus;
    int critBonus;

    public Focus(){
        stacks = 0;
    }

    //requires target
    public override float GetStat(StatType type, Unit actor = null, Unit target= null){
        if (type == StatType.acc && target.stats.modifiers.IsMarked){
            return markAccBonus;
        }
        else if (type == StatType.crit){
            return critBonus * stacks;
        } 
        return 0;
    }

    public override void OnAttack(Unit actor, Unit target)
    {
        if (target.stats.modifiers.IsMarked){
            stacks += 1;
        }
    }

    public override void OnCrit(){
        stacks = 0;
    }

    public override string ToString(){
        return string.Format(
            "{0} <color={4}><b>Focus</b></color>: Gives +{1}% Crit per stack (+{2}% Crit)\n" +
            "Focus: Gain +{3} Accuracy when attacking marked targets.\n",
            stacks, critBonus, critBonus*stacks, markAccBonus, 
            ColorPallete.GetHexColor("Red"));
    }
}
