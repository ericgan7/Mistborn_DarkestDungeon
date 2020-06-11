using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Dialogue/MetalBar Reward")]
public class MetalReward : Reward
{
    public int amount;
    public override bool IsState => true;

    public override void GetStateRewards(GameState state){
        state.am.UpdateMetal(amount);
    }
}
