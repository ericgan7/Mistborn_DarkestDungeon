using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Amount")]
public class ItemInstance : ScriptableObject
{
    public Item item;
    public int amount;

    public void Init(Item i, int a){
        item = i;
        amount = a;
    }
}

public class ItemAmount{
    public Item item;
    public int amount;

    public ItemAmount(ItemInstance i){
        item = i.item;
        amount = i.amount;
    }
    public ItemAmount(Item i, int a){
        item = i;
        amount = a;
    }
}
