using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Affliction_Paranoid : Affliction
{
    public override bool CanBuff => false;
    public override string AfflictionName => "Paranoid";

    int debuffResist = 10;
    public override float GetStat(StatType type, Unit actor = null, Unit target = null){
        if (type == StatType.debuffResist){
            return debuffResist;
        }
        return 0f;
    }

    public override string ToString(){
        return string.Format("{0}: Dodge +{1}, cannot recieve buffs\n", AfflictionName, debuffResist);
    }
}
