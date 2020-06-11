using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Test/Inventory")]
[System.Serializable]
public class Inventory : ScriptableObject
{
    public List<ItemInstance> items;

    public void UpdateInventory(List<ItemSlot> slots){

    }

    public bool Contains(Item item){
        foreach(ItemInstance i in items){
            if (i.itemType == item){
                return true;
            }
        }
        return false;
    }

    public void UseItem(Item item){
        foreach(ItemInstance i in items){
            if (i.itemType == item){
                i.RemoveAmount(1);
            }
        }
    }
}
