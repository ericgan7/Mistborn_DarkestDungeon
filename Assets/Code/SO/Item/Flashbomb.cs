using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Item/Flash bomb")]
public class Flashbomb : UsableItem
{
    public override bool IsUsable(Unit currrent, bool isCombat = true) {
        return isCombat;
    }
    public int stunAmount;

    public override void UseItem(Unit target, GameState gs){
        foreach (Unit u in gs.enemy.GetUnits()){
            u.stats.ApplyDelayedEffect(EffectLibrary.GetEffect("Stun") as StatusEffect);
        }
    }

    public override string ToString(){
        return string.Format("{0}\nEffect:Applies stun to all enemy units\nValue: {1}", itemName, value);
    }
}
