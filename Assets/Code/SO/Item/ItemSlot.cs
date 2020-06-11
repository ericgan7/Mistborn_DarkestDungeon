using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public Image background;
    public ItemObj currentItem;

    public bool temporary;
    public RewardBox tempParent;
    public bool equipOnly;

    public void SwapItems(ItemSlot target){
        if (target.currentItem != null && currentItem != null &&
        currentItem.item.itemType == target.currentItem.item.itemType){
            int remaining = currentItem.item.AddAmount(target.currentItem.item.amount);
            if (remaining > 0) {
                target.currentItem.item.amount = remaining;
            } else {
                target.currentItem = null;
                target.UpdateItemPosition();
            }
        } else {
            ItemObj temp = currentItem;
            currentItem = target.currentItem;
            target.currentItem = temp;
            target.UpdateItemPosition();
        }
        UpdateItemPosition();
    }

    public void UpdateItemPosition(){
        if (currentItem == null){
            if (temporary){
                tempParent.CheckClose();
                Destroy(gameObject);
            }
            return;
        }
        currentItem.slot = this;
        currentItem.transform.parent = transform;
        currentItem.rt.anchoredPosition = Vector3.zero;
    }

}
