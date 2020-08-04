using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ItemObjStore : ItemObj, IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI cost;

    public override void UpdateItem(Item item, int newAmount){
        cost.text = item.value.ToString();
        base.UpdateItem(item, newAmount);
    }

    public override int AddAmount(int extra){
        int remaining = base.AddAmount(extra);
        if (amount > 0){
            icon.color = Color.white;
        }
        return remaining;
    }

    void BuyItem(int amt){
        int removed = Mathf.Min(amount, amt);
        amount -= removed;
        if (amount <= 0){
            icon.color = ColorPallete.GetColor("Grey");
        }
        count.text = amount.ToString();
        ItemAmount instance = new ItemAmount(item, removed);
        GameEvents.current.BuyTrigger(instance);

    }

    public void OnPointerClick(PointerEventData pointer){
        if (amount <= 0){
            return;
        }   
        if (pointer.button == PointerEventData.InputButton.Left){
            BuyItem(1);
        } else if (pointer.button == PointerEventData.InputButton.Right){
            BuyItem(item.max_amount);
        }
    }
}
