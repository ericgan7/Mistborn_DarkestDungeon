using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poise : Traits
{
    public override string traitName => "Poise";
    int accuracy;
    int crit;

    int speed;
    int dodge;
    bool high;

    public Poise(){
        accuracy = 10;
        crit = 10;
        speed = 2;
        dodge = 20;
        high = true;
    }

    public override int ApplyOverTime(Unit target){
        high = target.stats.Defense().x > 0;
        return 0;
    }

    public override float GetStat(StatType type, Unit actor = null, Unit target = null){
        high = actor.stats.Defense().x > 0;
        if (type == StatType.acc && high){
            return accuracy;
        } else if (type == StatType.crit && high){
            return crit;
        } else if (type == StatType.speed && !high){
            return speed;
        } else if (type == StatType.dodge && !high){
            return dodge;
        } return 0;
    }

    public override string ToString(){
        if (high){
            return string.Format(
            "{0} <color={3}><b>Poise</b></color>: Grants +{1} Accuracy, +{2}% Crit.\n",
            "Agressive", accuracy, crit,
            ColorPallete.GetHexColor("Highlight Orange"));
        }
        else {
            return string.Format(
            "{0} <color={3}><b>Poise</b></color>: Grants +{1} Speed, +{2} Dodge.\n",
            "Evasive", speed, dodge,
            ColorPallete.GetHexColor("Highlight Teal"));
        }
        
    }
}
