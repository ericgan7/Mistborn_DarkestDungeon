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
    }

    public override string Stats()
    {
        return string.Format("Move {0} {1}\n", 
            Mathf.Abs(amount), 
            amount > 0 ? "Forward" : "Backward");
    }
}
