using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Defend : Effect
{
    int amount;
    public Defend(int a)
    {
        amount = a;
    }

    public override void ApplyEffect(Unit actor, Unit target, ref AbilityResultList results)
    {
        //TODO: roll for crit def
        Vector2Int def = actor.stats.Defense();
        AbilityResult r = new AbilityResult()
        {
            actor = actor,
            target = target,
            amount = Mathf.Min(amount, def.y - def.x),
            result = Result.def
        };
        results.targets.Add(r);
    }

    public override string ToString(){
        return string.Format("Gain {0} <color={1}><b>Defense</b></color>\n", 
            amount, ColorPallete.GetHexColor("Blue"));
    }
}
