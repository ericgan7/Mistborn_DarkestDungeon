using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inflame : Traits
{
    public override string traitName => "Inflame";
    int bonusDamage;
    int bonusCrit;
    public Inflame(){}

    public override float GetStat(StatType type, Unit actor = null, Unit target = null){
        if (target == null){
            return 0;
        }
        if (type == StatType.damage && target.stats.modifiers.IsBleed){
            return bonusDamage;
        }
        else if (type == StatType.crit && target.stats.modifiers.IsBleed){
            return bonusCrit;
        }
        return 0;
    }

    public override string ToString(){
        return string.Format(
            "<color={2}><b>Inflame</b></color>: +{0} Damage, +{1}% Crit when attacking bleeding targets.\n",
            bonusDamage, bonusCrit,
            ColorPallete.GetHexColor("Highlight Red"));
    }
}
