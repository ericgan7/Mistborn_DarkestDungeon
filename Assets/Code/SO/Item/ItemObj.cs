using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemObj : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image icon;
    [SerializeField] TextMeshProUGUI tooltip;
    [SerializeField] RectTransform background;
    [SerializeField] protected TextMeshProUGUI count;
    [SerializeField] protected RectTransform rt;
    [SerializeField] bool isLimited;

    protected Item item;
    public ItemSlot parentSlot;
    protected int amount;
    protected GraphicRaycaster raycaster;

    public Item Item {get { return item;} }
    public ItemSlot Slot {get {return parentSlot; }}


    public void SetRaycaster(GraphicRaycaster caster){
        raycaster = caster;
    }

    public void UpdateItem(ItemAmount instance)
    {
        if (instance == null || instance.item == null){
            Debug.Log("disabled");
            count.text = "";
            icon.enabled = false;
            return;
        }
        item = instance.item;
        icon.sprite = item.icon;
        icon.enabled = true;
        amount = instance.amount;
        count.text = instance.amount.ToString();
        SetTooltip();
    }

    public virtual void UpdateItem(Item newItem, int newAmount){
        item = newItem;
        icon.sprite = newItem.icon;
        icon.enabled = true;
        amount = newAmount;
        count.text = amount.ToString();
        SetTooltip();
    }

    public void SetTooltip(){
        background.gameObject.SetActive(true);
        if (item == null){
            tooltip.text = "";
        }
        else {
            tooltip.text = item.ToString();
        }
        background.sizeDelta = new Vector2(LayoutUtility.GetPreferredWidth(tooltip.rectTransform),
                                            LayoutUtility.GetPreferredHeight(tooltip.rectTransform));
        background.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData pointer){
        background.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData pointer){
        background.gameObject.SetActive(false);
    }

    public void SetIconUsable(Unit currentUnit, bool isCombat){
        if (item != null && item.IsUsable(currentUnit, isCombat)){
            icon.color = Color.white;
        } else {
            icon.color = new Color(1f,1f,1f, 0.5f);
        }
    }

    public bool SameItem(ItemObj other){
        if (other == null){
            return false;
        }
        return item == other.item;
    }

    public int AddAmount(ItemObj other){
        amount = amount + other.amount;
        if (isLimited && amount > item.max_amount){
            int remaining = amount - item.max_amount;
            amount = item.max_amount;
            count.text = amount.ToString();
            return remaining;
        }
        count.text = amount.ToString();
        return 0;
    }

    public virtual int AddAmount(int extra){
        amount += extra;
        if (isLimited && amount > item.max_amount){
            int remaining = amount - item.max_amount;
            amount = item.max_amount;
            count.text = amount.ToString();
            return remaining;
        }
        count.text = amount.ToString();
        return 0;
    }

    public void SetAmount(int amt){
        amount = amt;
        count.text = amount.ToString();
        if (isLimited && amount > item.max_amount){
            Debug.Log("Warning: Item is over limit");
        }
    }

    public int RemoveAmount(int extra){
        amount -= extra;
        if (extra <= 0){
            parentSlot.SetItem(null);
            Destroy(gameObject);
            return Mathf.Abs(amount);
        }
        count.text = amount.ToString();
        return 0;
    }

    public void SetSlot(ItemSlot slot){
        parentSlot = slot;
        transform.SetParent(slot.transform);
        rt.anchoredPosition = Vector3.zero;
        rt.localScale = Vector3.one;   
    }

    protected void RaycastCheck(PointerEventData p){
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(p, results);
        foreach (RaycastResult r in results){
            ItemSlot s = r.gameObject.GetComponent<ItemSlot>();
            if (s != null && s != parentSlot && !s.temporary){
                bool newItem = parentSlot.temporary;
                int current = amount;
                int remaining = s.SwapItems(parentSlot);
                if (newItem){
                    GameEvents.current.AddTrigger(new ItemAmount(item, current - remaining));
                }
                return;
            }
        }
        parentSlot.UpdateItemPosition();
    }
}

public class ItemCompare: Comparer<ItemObj>{
    public override int Compare(ItemObj x, ItemObj y){
        if (x.Slot.slotId < y.Slot.slotId){
            return -1;
        } else if (x.Slot.slotId > y.Slot.slotId){
            return 1;
        } else {
            return 0;
        }
    }
}
