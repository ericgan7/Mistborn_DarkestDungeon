using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemObjInventory : ItemObj, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    bool isDragging = false;

    public void OnPointerClick(PointerEventData pointer){
        if (pointer.button == PointerEventData.InputButton.Left){
            //?
        } else if (pointer.button == PointerEventData.InputButton.Right){
            ItemAmount instance = new ItemAmount(item, 1);
            Debug.Log("Sell Item");
            GameEvents.current.SellTrigger(instance);
            --amount;
            count.text = amount.ToString();
            if (amount <= 0){
                //inform others to remove this object
                parentSlot.SetItem(null);
                parentSlot.parentTray.RemoveItem(this);
                Destroy(gameObject);
            }
        }
    }

    public void OnPointerDown(PointerEventData p)
    {
        if (p.button == PointerEventData.InputButton.Left){
            isDragging = true;
            parentSlot.BringToFront();
        }
    }

    public void OnPointerUp(PointerEventData p){
        isDragging = false;
        RaycastCheck(p);
    } 

    void Update(){
        if (isDragging){
            rt.position = Input.mousePosition;
        }
    }
}
