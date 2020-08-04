using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Affliction : Traits
{
    public virtual bool CanBuff { get { return true; } }
    public virtual string AfflictionName { get { return ""; }}

    public virtual void OnAffliction(Unit actor){ }

    //use this function to do turn effects
    //public override void OnTurnStart(Unit actor){}

    static protected int stressDamage = 1;
    public virtual IEnumerator StressTeam(Unit actor){
        foreach(Unit u in actor.UnitTeam.GetUnits()){
            if ( u == actor){
                continue;
            }
            u.stats.StressDamage(stressDamage);
            u.CreatePopUpText(stressDamage.ToString(), ColorPallete.GetColor("Purple"));
            yield return new WaitForSeconds(0.2f);
        }
    }

    static protected int stressHeal = -2;
    protected IEnumerator RecoverTeam(Unit actor){
        foreach(Unit u in actor.UnitTeam.GetUnits()){
            if (u == actor){
                continue;
            }
            u.stats.StressDamage(stressHeal);
            u.CreatePopUpText(stressHeal.ToString(), ColorPallete.GetColor("White"));
            yield return new WaitForSeconds(0.2f);
        }
    }

    public bool AfflictionTurnChance(){
        int chance = Mathf.FloorToInt(Random.Range(0f, 10f));
        if (chance < 1){
            return true;
        }
        return false;
    }
}
