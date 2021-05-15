using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    void Awake(){
        current = this;
    }

    //events
    public event Action<ItemAmount> onBuyItem;
    public void BuyTrigger(ItemAmount instance){
        if (onBuyItem != null && 
            GameState.Instance.inventory.HasRoom(instance) && 
            ResourceManager.HasResource(ResourceType.gold, instance.item.value)){
            onBuyItem(instance);
        }
    }

    public event Action<ItemAmount> onAddItem;
    public void AddTrigger(ItemAmount instance){
        if (onAddItem != null){
            onAddItem(instance);
        }
    }

    public event Action<ItemAmount> onSellItem;
    public void SellTrigger(ItemAmount instance){
        if (onSellItem != null){
            onSellItem(instance);
        }
    }

    public event Action onChangeMetal;
    public void MetalTrigger(){
        if (onChangeMetal != null){
            onChangeMetal();
        }
    }

    public event Action<Ability> onSelectAbility;
    public void SelectAbilityTrigger(Ability ability){
        if (onSelectAbility != null){
            onSelectAbility(ability);
        }
    }
}
