using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemObjCombat : ItemObj, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    bool isDragging = false;

    public void OnPointerClick(PointerEventData p){
        //shift right click drops items
        if (Input.GetKey(KeyCode.LeftShift) && p.button == PointerEventData.InputButton.Right){
            GameEvents.current.SellTrigger(new ItemAmount(item, amount));
            parentSlot.SetItem(null);
            Destroy(gameObject);
        }
        else if (p.button == PointerEventData.InputButton.Right && GameState.Instance.gc.currentUnit.UnitTeam.isAlly){
            if (!item.IsUsable(GameState.Instance.gc.currentUnit, GameState.Instance.gc.mode == GameMode.combat)) {
                return;
            }
            //apply item effects
            item.UseItem(GameState.Instance.gc.currentUnit);
            //use item
            --amount;
            GameEvents.current.SellTrigger(new ItemAmount(item, 1));
            if (amount <= 0){
                parentSlot.SetItem(null);
                Destroy(gameObject);
            } else {
                count.text = amount.ToString();
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
