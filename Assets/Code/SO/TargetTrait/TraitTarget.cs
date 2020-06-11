using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class TraitTarget{
    public virtual EffectType Trait {get {return EffectType.none; }}
    protected int bonus = 0;

    public TraitTarget(int init_bonus){
        bonus = init_bonus;
    }

    public int GetBonusDamage(Unit target){
        //need to to check character traits and status effects, depending.
        foreach(Traits e in target.stats.GetTraits()){
            if (e.Type == Trait){
                return bonus;
            }
        }
        return 0;
    }

    public override string ToString(){
        return "";
    }
}