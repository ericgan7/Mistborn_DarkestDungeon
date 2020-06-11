using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Affliction_Vengeful : Affliction
{
    public override string  AfflictionName => "Vengeful";

    int damageBonus = 1;
    int critBonus = 10;
    public override float GetStat(StatType type, Unit actor = null, Unit target = null){
        if (type == StatType.damage){
            return damageBonus;
        }
        if (type == StatType.crit){
            return critBonus;
        }
        return 0;
    }

    public override IEnumerator StressTeam(Unit actor){
        yield return RecoverTeam(actor);
    }

    public override string ToString(){
        return string.Format("{0}: Damage +{1}, Crit +{2}", AfflictionName, damageBonus, critBonus);
    }
}
