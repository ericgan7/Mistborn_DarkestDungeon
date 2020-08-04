using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTrait : Traits
{
    string tName;
    StatType stats;
    int amount;
    TraitCondition condition;

    public override string traitName => tName;

    public StatTrait(string name, StatType stat, int amt, TraitCondition cond){
        tName = name;
        stats = stat;
        amount = amt;
        condition = cond;
    }

    public override float GetStat(StatType type, Unit actor = null, Unit target = null){
        if (condition != null && condition.isActive(target) == false){
            return 0f;
        }

        return 0f;
    }
}

public class TraitCondition{
    EffectType stat;
    int amount;
    bool isSelf;
    public TraitCondition(EffectType st, int amt, bool self){
        stat = st;
        amount = amt;
        isSelf = self;
    }

    public bool isActive(Unit target){
        //special check for alarm/ round turn
        if (target.stats.modifiers.HasEffect(stat)){
            return true;
        }
        return false;
    }

    public int GetAmount(){
        return amount;
    }
}
