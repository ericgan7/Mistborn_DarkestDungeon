using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Affliction_Machoistic : Affliction
{
    public override void OnAffliction(Unit actor){
        actor.stats.modifiers.AddEffect(EffectLibrary.GetEffect("Mark 3") as StatusEffect);
        Vector2Int defense = actor.stats.Defense();
        int gain = defense.y - defense.x;
        actor.stats.GainArmor(gain);
        actor.CreatePopUpText(gain.ToString(), Color.blue);
    }

    int dodgeDebuff = -50;
    public override float GetStat(StatType type, Unit actor = null, Unit target = null){
        if (type == StatType.dodge){
            return dodgeDebuff;
        }
        return 0f;
    }

    public override string ToString(){
        return string.Format("{0}: Mark Self, Regain all defense, Dodge - {1}\n", AfflictionName, dodgeDebuff);
    }
}
