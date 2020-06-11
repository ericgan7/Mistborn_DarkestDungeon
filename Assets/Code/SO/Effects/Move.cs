using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Effect
{
    public override void ApplyDelayedEffect(Unit actor, Unit target)
    {
        Debug.Log(actor.Location);
        Debug.Log(target.Location);
        int distance = target.Location - actor.Location;
        actor.UnitTeam.MoveUnit(actor, distance);
    }
}

public class MoveSelf : Effect{
    public override void ApplyDelayedEffect(Unit actor, Unit target)
    {
        int distance = target.Location - actor.Location;
        actor.UnitTeam.MoveUnit(actor, distance);
    }
}
