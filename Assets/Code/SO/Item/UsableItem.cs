using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Item/Usable Item")]
public class UsableItem : Item
{
    public EffectType effectTarget;
    public Ability_Item effect;
    public bool isUsbleInCombat;

    public override bool IsUsable(Unit current, bool isCombat = true)
    {
        return current.stats.modifiers.HasEffect(effectTarget) && (isUsbleInCombat || isCombat == isUsbleInCombat);
    }

    public override void UseItem(Unit actor){
        AbilityResultList temp = new AbilityResultList();
        effect.ApplyAbility(actor, ref temp);
    }

    public override string ToString(){
        string baseline = string.Format("{0}Effect:\n", base.ToString());
        foreach(Effect e in effect.Effects){
            baseline += e.ToString();
        }
        return baseline;
    }
}

