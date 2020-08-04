using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Momentum : Traits
{
    public override string traitName => "Momentum";
    int stacks;
    bool hasMoved;

    int dodge = 5;
    int damage = 1;

    public Momentum(){
        stacks = 0;
    }
    
    public override float GetStat(StatType type, Unit actor = null, Unit target = null){
        if (type == StatType.dodge){
            return stacks * dodge;
        } else if (type == StatType.damage){
            return stacks * damage;
        }
        return 0f;
    }

    public override void OnMove(Unit actor) 
    { 
        stacks += 1;
        hasMoved = true;
        //spawn text?
    }

    public override int ApplyOverTime(Unit actor){
        if (actor.stats.modifiers.IsStunned){
            stacks = 0;
        } else if (!hasMoved) {
            stacks = Mathf.Clamp(stacks - 1, 0, 10);
        }
        return 0;
    }

    public override string ToString(){
        return string.Format(
            "{0} <color={5}><b>Momentum</b></color>: Gain 1 stack when moving. Gives +{1} Dodge, +{2} Damage per stack.\n",
            stacks, dodge, damage, stacks*dodge, stacks*damage,
            ColorPallete.GetHexColor("Highlight Teal"));
    }


}
