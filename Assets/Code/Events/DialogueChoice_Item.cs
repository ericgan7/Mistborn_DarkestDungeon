using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueChoice_Item : DialogueChoice
{
    [SerializeField] ItemSlot slot;

    public override void OnPointerClick(PointerEventData pointer){
        Debug.Log("Use Item");
        if (slot.currentItem != null){
            manager.SelectChoice(current, slot.currentItem.Item);
        }
    }
}
