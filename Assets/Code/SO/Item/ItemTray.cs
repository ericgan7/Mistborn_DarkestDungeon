using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTray : MonoBehaviour
{
    List<ItemSlot> slots = new List<ItemSlot>();

    [SerializeField] int width;
    [SerializeField] int minimum_rows;
    [SerializeField] Transform content;
    [SerializeField] ItemObj itemPrefab;
    [SerializeField] ItemSlot slotPrefab;
    [SerializeField] GraphicRaycaster raycaster;
    [SerializeField] float topOffset;
    [SerializeField] float leftOffset;
    [SerializeField] float spacing;
    Dictionary<Item, List<ItemObj>> lookup = new Dictionary<Item, List<ItemObj>>();
    static ItemCompare comparer = new ItemCompare();
    public bool init = false;
    bool isActive = false;

    void Awake(){ 
        Init();
    }

    public void Init(){
        if (init){
            return;
        }
        for (int y = 0; y < minimum_rows; ++y){
            for (int x = 0; x < width; ++x){
                ItemSlot slot = Instantiate<ItemSlot> (slotPrefab, content);
                slot.Init(y * x + x, this);
                slot.SetPosition(new Vector2(leftOffset + (slot.Size.x + spacing) * x, -topOffset - (slot.Size.y + spacing) * y));
                slots.Add(slot);
            }
        }
        init = true;
    }

    public void Activate(bool isOn){
        isActive = isOn;
        gameObject.SetActive(isOn);
        gameObject.transform.SetAsLastSibling();
    }

    public void Click(){
        isActive = !isActive;
        Activate(isActive);
    }

    public void SetIconUsable(Unit currentUnit, GameMode mode){
        foreach(ItemSlot i in slots){
            if (i.currentItem != null){
                //i.currentItem.SetIconUsable(currentUnit, mode == GameMode.combat);
            }
        }
    }

    public void UpdateInventory(List<ItemAmount> items){ //may need to create new slots;
        Init();
        for (int i = 0; i < items.Count; ++i){
            CreateItemObj(items[i].item, items[i].amount, slots[i]);
        }
    }

    public void RemoveItem(ItemAmount instance){
        if (!lookup.ContainsKey(instance.item)){
            Debug.Log("Tried to remove none-existant item from tray");
            return;
        }
        int remaining = instance.amount;
        int removed = 0;
        foreach (ItemObj item in lookup[instance.item]){
            remaining = item.RemoveAmount(remaining);
            if (remaining >= 0){
                ++removed;
            } else {
                break;
            }
        }
        slots.RemoveRange(0, removed);
        if (slots.Count == 0){
            Debug.Log("remove Item");
            lookup.Remove(instance.item);
        }
    }
    
    public void RemoveItem(ItemObj obj){
        if (!lookup.ContainsKey(obj.Item)){
            Debug.Log("Trying to remove non existant itemobj from tray");
            return;
        }
        foreach(ItemObj i in lookup[obj.Item]){
            if (i == obj){
                lookup[obj.Item].Remove(i);
                break;
            }
        }
    }

    public void AddItem(ItemAmount instance){
        int remaining = instance.amount;
        if (lookup.ContainsKey(instance.item)){
            foreach(ItemObj item in lookup[instance.item]){
                remaining = item.AddAmount(remaining);
                if (remaining <= 0){
                    break;
                }
            }
        }
        if (remaining > 0){
            foreach(ItemSlot slot in slots){
                if (slot.HasItem()){
                    continue;
                }
                CreateItemObj(instance.item, Mathf.Min(remaining,instance.item.max_amount), slot);
                remaining -= instance.item.max_amount;
                if (remaining <= 0){
                    break;
                }
            }
        }
    }

    void CreateItemObj(Item item, int amount, ItemSlot slot){
        ItemObj obj = Instantiate<ItemObj>(itemPrefab, slot.transform);
        obj.UpdateItem(item, amount);
        obj.SetRaycaster(raycaster);
        slot.SetItem(obj);
        if (!lookup.ContainsKey(item)){
            lookup.Add(item, new List<ItemObj>());
        }
        lookup[item].Add(obj);
        lookup[item].Sort(comparer);
    }
}
