using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Item/Metal Vials")]
public class MetalItem: UsableItem
{
    public override bool IsUsable(Unit currrent, bool isCombat = true) {
        return true;
    }
    public int metalAmount;
    public override void UseItem(Unit target, GameState gs){
        gs.am.UpdateMetal(metalAmount);
    }

    public override string ToString(){
        return string.Format("{0}\nEffect:Adds {1} to metal reserve\nValue: {2}\n",
         itemName, metalAmount, value);
    }
}
