using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Affliction_Fearful : Affliction
{
    public override string AfflictionName => "Fearful";
    
    public override void OnAffliction(Unit actor){
        actor.stats.StressDamage(5);
        actor.UnitTeam.MoveUnit(actor, -3);
    }

    int dodgeBuff = 10;
    public override float GetStat(StatType type, Unit actor = null, Unit target = null){
        if (type == StatType.dodge){
            return dodgeBuff;
        }
        return 0f;
    }

    public override string ToString(){
        return string.Format("{0}: Dodge +{1}", AfflictionName, dodgeBuff);
    }
}
