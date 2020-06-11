using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Affliction_Protective : Affliction
{
    public override string AfflictionName => "Protective";

    public override void OnAffliction(Unit actor){
        Vector2Int defense = actor.stats.Defense();
        int gain = defense.y - defense.x;
        actor.stats.GainArmor(gain);
        actor.CreatePopUpText(gain.ToString(), Color.blue);
    }

    int armorGain = 2;
    public override IEnumerator StressTeam(Unit actor){
        foreach(Unit u in actor.UnitTeam.GetUnits()){
            if (u==actor){
                continue;
            }
            u.stats.GainArmor(armorGain);
            u.CreatePopUpText(armorGain.ToString(), Color.blue);
            yield return new WaitForSeconds(0.2f);
        }

        yield return RecoverTeam(actor);
    }

    public override string ToString(){
        return string.Format("{0}: Regain all defense, Chance to defend teammates\n", AfflictionName);
    }
}
