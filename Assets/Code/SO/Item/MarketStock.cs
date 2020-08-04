using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Create Market")]
public class MarketStock : ScriptableObject
{
    [SerializeField] List<ItemInstance> items;

    public List<ItemAmount> GetItems(){
        List<ItemAmount> itemAmount = new List<ItemAmount>();
        foreach (ItemInstance i in items){
            itemAmount.Add(new ItemAmount(i));
        }
        return itemAmount;
    }
}
