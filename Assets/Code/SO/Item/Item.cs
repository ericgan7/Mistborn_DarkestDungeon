using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Value Item")]
[System.Serializable]
public class Item : ScriptableObject
{   
    public string itemName;
    public Sprite icon;
    public int value;
    public int max_amount;
    public virtual bool IsUsable (Unit currrent, bool isCombat = true) {return false;}
    public virtual bool IsEquipable { get {return false;}}

    public virtual void UseItem(Unit actor){}
    public virtual void EquipItem(Unit actor) {}

    public override string ToString(){
        return string.Format("{0}\nValue: {1}\n", itemName, value);
    }
}

public static class ItemLibrary 
{
    public static List<ItemAmount> GenerateAmounts(List<Item> rewards, int value){
        List<ItemAmount> items = new List<ItemAmount>();
        int roll;
        foreach (Item item in rewards){
            roll = Random.Range(0, Mathf.FloorToInt((float) value / (float) item.value));
            int total = Mathf.CeilToInt((float) roll / (float)item.max_amount);
            for (int i = 0; i < total; ++i){
                ItemAmount instance  = new ItemAmount(item, Mathf.Min(item.max_amount, roll));
                roll -= item.max_amount;
                items.Add(instance);
            }
        }
        return items;
    }
}