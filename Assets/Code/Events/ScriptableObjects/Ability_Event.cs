using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Event/Ability Effects")]
public class Ability_Event : Ability_Attack
{

    public override int GetDamage(Unit actor, Unit target){
        return (int) dmgmod;  
    }

    public override void ApplyAbility(Unit actor, Unit target, ref AbilityResultList results){
        if (dmgmod > 0){
            int roll = Random.Range(0, 100);
            if (isAOE){
                foreach(Unit u in target.UnitTeam.GetUnits()){
                    ApplyDamage(actor, u, roll, ref results);
                }
            }
            else {
                ApplyDamage(actor, target, roll, ref results);
            }
        }
        if (isAOE){
            foreach(Unit u in target.UnitTeam.GetUnits()){
                foreach (Effect e in TargetBuffs){
                    e.ApplyEffect(actor, u, ref results);
                }
            }
        } else {
            foreach (Effect e in TargetBuffs){
                e.ApplyEffect(actor, target, ref results);
            }
        }
        results.counter.Clear();
    }
}
