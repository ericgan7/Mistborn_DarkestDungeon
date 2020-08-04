using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour
{
    [SerializeField] ItemTray itemDisplay;
    [SerializeField] MarketStock stock;
    [SerializeField] ResourceManager resource;

    void Start(){
        itemDisplay.UpdateInventory(stock.GetItems());
        //placeholder
        Activate(true);
    }

    public void Activate(bool isOn){
        if (isOn){
            GameEvents.current.onBuyItem += BuyItem;
            GameEvents.current.onSellItem += SellItem;
        } else {
            GameEvents.current.onBuyItem -= BuyItem;
            GameEvents.current.onSellItem -= SellItem;
        }
        gameObject.SetActive(isOn);
    }

    public void BuyItem(ItemAmount instance){
        ResourceManager.SpendResource(ResourceType.gold, -1 * instance.item.value * instance.amount);
        //bought item will remove itself from the  store, since it was the trigger
        resource.UpdateUI();
        //can add sound queue;
    }

    public void SellItem(ItemAmount instance){
        ResourceManager.SpendResource(ResourceType.gold, instance.item.value * instance.amount);
        itemDisplay.AddItem(instance);
        resource.UpdateUI();
    }



}
