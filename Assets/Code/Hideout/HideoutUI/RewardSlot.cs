using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class RemoveSlotEvent :UnityEvent<int>{}

public class RewardSlot : ItemSlot
{
    RemoveSlotEvent removed;
    int index;
    
    public void AddCallBack(RemoveSlotEvent unityEvent){
        removed = unityEvent;
    }

    public void SetIndex(int i){
        index = i;
    }

    public override int SwapItems(ItemSlot target){
        int remain = base.SwapItems(target);
        if (currentItem == null){
            removed.Invoke(index);
        }
        return remain;
    }
}
