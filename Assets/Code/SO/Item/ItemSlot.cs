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
    public int slotId;

    public bool temporary;
    //public RewardBox tempParent;
    public bool equipOnly;
    public ItemTray parentTray;
    [SerializeField] RectTransform rectTransform;
    public Vector2 Size {get {return rectTransform.sizeDelta; }}

    public void Init(int id, ItemTray tray){
        slotId = id;
        parentTray = tray;
    }

    public void SetPosition(Vector2 pos){
        rectTransform.anchoredPosition = pos;
    }

    public void SetItem(ItemObj obj){
        currentItem = obj;
        UpdateItemPosition();
    }

    public virtual int SwapItems(ItemSlot target){
        Debug.Log("swaping");
        int remaining = 0;
        if (currentItem != null && currentItem.SameItem(target.currentItem)){
            remaining = currentItem.AddAmount(target.currentItem);
            Debug.Log(remaining);
            if (remaining > 0) {
                target.currentItem.SetAmount(remaining);
            } else {
                target.currentItem = null;
            }
        } else {
            ItemObj temp = currentItem;
            currentItem = target.currentItem;
            target.currentItem = temp;
        }
        UpdateItemPosition();
        target.UpdateItemPosition();
        return remaining;
    }

    public void UpdateItemPosition(){  
        if (currentItem == null){
            if (temporary){
                //tempParent.CheckClose();
                Destroy(gameObject);
            }
            return;
        }
        else {
            currentItem.SetSlot(this);
        }
        
    }

    public bool HasItem(){
        return currentItem != null;
    }

    public void BringToFront(){
        transform.SetAsLastSibling();
    }

}

