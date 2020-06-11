using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTray : MonoBehaviour
{
    public List<ItemSlot> slots;
    public Inventory inventory;

    public ItemObj prefab;

    void Awake(){ 
        slots = new List<ItemSlot>(gameObject.GetComponentsInChildren<ItemSlot>());
        for (int i = 0; i < inventory.items.Count; ++i){
            ItemObj obj = Instantiate<ItemObj>(prefab, slots[i].transform);
            obj.UpdateItem(inventory.items[i]);
            slots[i].currentItem = obj;
            slots[i].UpdateItemPosition();
        }
    }

    public void SetIconUsable(Unit currentUnit, GameMode mode){
        foreach(ItemSlot i in slots){
            if (i.currentItem != null){
                i.currentItem.SetIconUsable(currentUnit, mode == GameMode.combat);
            }
        }
    }

    public void UpdateInventory(){
        
    }
}
