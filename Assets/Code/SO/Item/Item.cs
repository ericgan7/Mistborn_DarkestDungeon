using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Gold")]
[System.Serializable]
public class Item : ScriptableObject
{   
    public string itemName;
    public Sprite icon;
    public int value;
    public int max_amount;
    public virtual bool IsUsable (Unit currrent, bool isCombat = true) {return false;}
    public virtual bool IsEquipable { get {return false;}}

    public override string ToString(){
        return string.Format("{0}\nValue: {1}\n", itemName, value);
    }
}

public abstract class UsableItem : Item
{
    public bool IsSelf;
    public abstract void UseItem(Unit target, GameState gs);
}

[CreateAssetMenu(menuName="Test/ItemInstance")]
public class ItemInstance : ScriptableObject
{
    public Item itemType;
    public int amount;
    public ItemInstance(Item i, int a)
    {
        itemType = i;
        amount = a;
    }

    public int AddAmount(int additionalAmount)
    {
        amount += additionalAmount;
        if (amount > itemType.max_amount)
        {
            return amount - itemType.max_amount;
        }
        return 0;
    }

    public bool RemoveAmount(int removedAmount){
        amount -= removedAmount;
        if (amount > 0){
            return true;
        }
        return false;
    }

    public int UseItem(Unit currentUnit, GameState gs){
        if (itemType.IsUsable(currentUnit, true)){
            ((UsableItem)itemType).UseItem(currentUnit, gs);
            --amount;
        }
        return amount;
    }
}