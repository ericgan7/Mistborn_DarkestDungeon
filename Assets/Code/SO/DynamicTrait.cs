using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Character/Random Trait")]
public class DynamicTrait : ScriptableObject
{
    public string traitname;
    public StatType statType;
    public int amount;

    public EffectType condition;
    public int conditionAmount;
    public bool isSelf;

    public Traits GetTrait(){
        if (condition == EffectType.none){
            return new StatTrait(traitname, statType, amount, null);
        } else {
            TraitCondition c = new TraitCondition(condition, conditionAmount, isSelf);
            return new StatTrait(traitname, statType, amount, c);   
        } 
    }
}
