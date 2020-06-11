using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPull : Effect
{
    int amount; //positive is backward, negative is forward [0, 1, 2, 3] = [meleefront, meleeback, rangefront, rangeback]

    public PushPull(int a)
    {
        amount = a;
    }

    public override void ApplyDelayedEffect(Unit actor, Unit target)
    {
        target.UnitTeam.MoveUnit(target, -amount);
        target.stats.modifiers.UpdateOnAction(EffectType.move);
    }

    public override string ToString()
    {
        return string.Format("<color={2}><b>Move</b></color> {0} {1}\n", 
            Mathf.Abs(amount), amount > 0 ? "Forward" : "Backward", ColorPallete.GetHexColor("Teal"));
    }

    public override string ToString(Unit target){
        return string.Format("<color={1}><b>Move</b></color>: {0}% chance\n", 
            100 - target.stats.GetStat(StatType.moveResist), ColorPallete.GetHexColor("Teal"));
    }
}
