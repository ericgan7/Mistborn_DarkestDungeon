using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Gold")]
[System.Serializable]
public class Item : ScriptableObject
{
    public ItemType type;
    public Sprite icon;
    public int value;
    public int max_amount;
    public virtual bool IsUsable { get { return false; } }

    public virtual void UseItem(Unit target)
    {

    }
}

public abstract class UsableItem : Item
{
    public override bool IsUsable => true;
    public bool IsSelf;
}

public class ItemInstance
{
    public Item item;
    public int amount;
}
