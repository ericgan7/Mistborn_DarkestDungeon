using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Dialogue/Effect Reward")]
public class EffectReward : Reward
{
    public List<string> effects;
    public override bool IsCharacter => true;

    public override void GetCharacterRewards(Unit unit){
        foreach(string s in effects){
            EffectLibrary.GetEffect(s).ApplyDelayedEffect(unit, unit);
        }
    }
}
