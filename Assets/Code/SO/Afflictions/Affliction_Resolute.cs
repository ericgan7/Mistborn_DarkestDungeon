using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Affliction_Resolute : Affliction
{
    public override string AfflictionName => "Resolute";

    int debuffResist = 20;
    public override float GetStat(StatType type, Unit actor = null, Unit target = null){
        if (type == StatType.debuffResist){
            return debuffResist;
        }
        return 0;
    }

    public override string ToString(){
        return string.Format("{0}: Debuff resistance +{1}\n", AfflictionName, debuffResist);
    }

    public override IEnumerator StressTeam(Unit actor){
        yield return RecoverTeam(actor);
    }
}
