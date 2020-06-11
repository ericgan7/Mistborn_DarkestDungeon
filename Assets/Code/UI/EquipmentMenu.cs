using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentMenu : MonoBehaviour
{
    public List<ItemSlot> slots;
    
    public void Awake(){
        slots = new List<ItemSlot>(GetComponentsInChildren<ItemSlot>());
    }
    public void SetUnit(Unit unit){
        List<EquipableItem> items = unit.stats.GetItems();
        for (int i = 0; i < items.Count; ++i){
            slots[i].currentItem.UpdateItem(new ItemInstance(items[i], 0));
        }
    }
}
