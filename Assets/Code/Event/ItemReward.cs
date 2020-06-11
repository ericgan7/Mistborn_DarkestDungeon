using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Dialogue/Item Reward")]
public class ItemReward : Reward
{
    public Item item;
    public int amount;
    public override bool IsItem => true;
    public override ItemInstance GetItemRewards(){
        ItemInstance reward = new ItemInstance(item, amount);
        return reward;
    }
}
