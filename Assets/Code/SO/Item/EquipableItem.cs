using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Trinket")]
public class EquipableItem: Item {
    public override bool IsEquipable => true;
    public Ability effect;

    public List<string> itemStats;
    public List<StatusEffect> stats;

    public List<StatusEffect> GetStats(){
        if (stats != null){
            return stats;
        }
        stats = new List<StatusEffect>();
        foreach (string s in itemStats){
            stats.Add(EffectLibrary.GetEffect(s) as StatusEffect);
        }
        return stats;
    }

    public Ability GetAbility(){
        return effect;
    }

    public override string ToString(){
        string description = string.Format("{0}\nStats:\n", itemName);
        foreach (StatusEffect e in GetStats()){
            description += string.Format("{0}\n", e.ToString());
        }
        if (effect != null){
            description += string.Format("Adds Ability: {0}\n", effect.abilityName);
        }
        description += string.Format("Value: {0}\n", value);
        return description;
    }
}
