using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : StatusEffect
{
    Unit defendee;
    Unit defender;

    public Guard(int d){
        duration = d;
    }
    public override void ApplyDelayedEffect(Unit actor, Unit target)
    {
        defender = actor;
        defendee = target;
        target.stats.ApplyDelayedEffect(this);
    }

    public Unit GetTarget(){
        if (defender == null){
            Debug.Log("Guard is not initialized");
        }
        return defender;
    }

    public override string ToString(){
        return string.Format("<color={2}><b>Guarded</b></color> by {0} for {1} turns", 
            defender != null ? defender.stats.GetName() : "unit", duration, ColorPallete.GetHexColor("Blue"));
    }
}
