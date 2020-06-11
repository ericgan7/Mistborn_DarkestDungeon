using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Affliction_Righteous : Affliction
{
    public override string AfflictionName => "Righteous";

    int stressResist = 1;
    public override float GetStat(StatType type, Unit actor = null, Unit target = null){
        if (type == StatType.stressResist){
            return stressResist;
        }
        return 0;
    }

    public override string ToString(){
        return string.Format("{0}: +1 Stress resistance, Chance to recover stress on teammates", AfflictionName);
    }

    public override IEnumerator StressTeam(Unit actor){
        yield return RecoverTeam(actor);
    }
}
