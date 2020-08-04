using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constitution : Traits
{
    public override string traitName => "Constitution";
    int bonusHealth;
    int regen;

    public Constitution(){
        bonusHealth = 10;
        regen = 1;
    }

    public override float GetStat(StatType type, Unit actor = null, Unit target = null){
        if(type == StatType.health){
            return bonusHealth;
        }
        return 0;
    }

    public override int ApplyOverTime(Unit target){
        target.stats.Heal(regen);
        return regen;
    }

    public override string ToString(){
        return string.Format(
            "<color={2}><b>Constitution</b></color>: +{0} Max health, +{1} Heal per turn",
            bonusHealth, regen, 
            ColorPallete.GetHexColor("Green"));
    }
}
