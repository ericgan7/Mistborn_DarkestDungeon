using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueItemReward : DialogueText
{
    [SerializeField] RewardSlot slotPrefab;
    [SerializeField] ItemObj objPrefab;
    [SerializeField] Transform itemList;
    [SerializeField] DialogueList manager;
    [SerializeField] GraphicRaycaster raycast;
    List<ItemSlot> slots;
    RemoveSlotEvent removeSlot;

    public new void Awake(){
        Init();
    }

    public void AddItems(List<ItemAmount> rewards){
        if (slots == null){
            slots = new List<ItemSlot>();
        }
        if (removeSlot == null){
            removeSlot = new RemoveSlotEvent();
            removeSlot.AddListener(CheckSlots);
        }
        foreach(ItemAmount item in rewards){
            RewardSlot slot = Instantiate<RewardSlot>(slotPrefab, itemList);
            slot.SetIndex(slots.Count);
            slot.AddCallBack(removeSlot);
            slots.Add(slot);
            ItemObj obj = Instantiate<ItemObj>(objPrefab);
            obj.UpdateItem(item);
            obj.SetRaycaster(raycast);
            slot.SetItem(obj);
        }
        
    }

    //unity events
    public void CheckSlots(int index){
        slots.RemoveAt(index);
        if (slots.Count == 0){
            manager.CloseItems();
        }
    }
}
