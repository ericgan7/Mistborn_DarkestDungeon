using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public List<ItemInstance> items;

    public Inventory()
    {
        items = new List<ItemInstance>();
    }

    public void AddItem(ItemInstance newItem)
    {
        foreach( ItemInstance i in items)
        {
            if (i.item.type == newItem.item.type)
            {
                i.amount += newItem.amount;
                return;
            }
        }
        items.Add(newItem);
    }

    public void RemoveItem(Item oldItem, int amount)
    {
        foreach (ItemInstance i in items)
        {
            if (i.item.type == oldItem.type)
            {
                i.amount -= amount;
                if (i.amount == 0)
                {
                    items.Remove(i);
                }
                return;
            }
        }
    }
}
