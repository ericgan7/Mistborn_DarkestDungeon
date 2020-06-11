using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Dialogue/Reward/Scouting Reward")]
public class ScoutingReward : Reward
{
    public int change;
    public override bool IsState =>true;
    public override void GetStateRewards(GameState state){
        state.map.player.vision += change;
        state.map.UpdateMapState();
    }
}
