using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Test/Inventory")]
[System.Serializable]
public class Inventory : ScriptableObject
{
    Dictionary<Item, ItemAmount> items = new Dictionary<Item, ItemAmount>();
    [SerializeField] int maxSize;
    [SerializeField] List<ItemInstance> preset;

    public int Size {get {return maxSize; }}
    public Item ammo;   //reference to ammo item type
    public List<ItemAmount> Items { get {return new List<ItemAmount>(items.Values); }}

    public void Init(){
        if (preset != null && preset.Count > 0){
            foreach(ItemInstance i in preset){
                AddItem(new ItemAmount(i));
            }
        }
    }

    //can cache this result for better performance in the future
    public bool HasRoom(ItemAmount instance){
        int size = 0;
        bool extraSpace = true;
        foreach (ItemAmount i in items.Values){
            if (i.item == instance.item){
                size += Mathf.CeilToInt( (float) (i.amount + instance.amount) / (float) i.item.max_amount);
                extraSpace = false;
            } else {
                size += Mathf.CeilToInt((float) i.amount / (float) i.item.max_amount);
            }
        }
        if (!extraSpace){
            size += Mathf.CeilToInt((float) instance.amount / (float) instance.item.max_amount);
        }
        return size <= maxSize;
    }

    public void AddItem(ItemAmount instance){
        if (items.ContainsKey(instance.item)){
            items[instance.item].amount += instance.amount;
        } else {
            items.Add(instance.item, instance);
        }
    }

    public void RemoveItem(ItemAmount instance){
        if (items.ContainsKey(instance.item)){
            items[instance.item].amount -= instance.amount;
            if (items[instance.item].amount <= 0){
                items.Remove(instance.item);
            }
        }
    }

    public bool HasItem(ItemAmount instance){
        if (items.ContainsKey(instance.item)){
            return items[instance.item].amount >= instance.amount;
        }
        return instance.amount == 0;
    }
}
