using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] ItemTray items;
    
    void Start(){
        GameEvents.current.onBuyItem += BuyItem;
        GameEvents.current.onSellItem += RemoveItem;
        GameEvents.current.onAddItem += AddItem;
        InitInventory();
    }

    void InitInventory(){
        foreach (ItemAmount i in GameState.Instance.inventory.Items){
            items.AddItem(i);
        }
    }

    public void AddItem(ItemAmount instance){
        GameState.Instance.inventory.AddItem(instance);
    }

    public void BuyItem(ItemAmount instance){
        GameState.Instance.inventory.AddItem(instance);
        items.AddItem(instance);
    }

    public void RemoveItem(ItemAmount instance){
        GameState.Instance.inventory.RemoveItem(instance);
        //triggered from invetory sell, so we do not have to remove;
    }
}
