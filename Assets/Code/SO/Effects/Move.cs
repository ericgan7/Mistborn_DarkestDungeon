using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Effect
{
    public override void ApplyDelayedEffect(Unit actor, Unit target)
    {
        int distance = target.Location - actor.Location;
        target.UnitTeam.MoveUnit(target, distance);
    }
}
