using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Heal : Effect { 
    int amount;

    public Heal(int a)
    {
        amount = a;
    }

    public override void ApplyEffect(Unit actor, Unit target, ref AbilityResultList results)
    {
        AbilityResult r = new AbilityResult()
        {
            actor = actor,
            target = target,
            amount = amount,
            result = Result.heal
        };
        results.targets.Add(r);
    }

    public override string ToString(Unit target)
    {
        return string.Format("<color={1}><b>Heal</b></color> {0} health\n", amount, ColorPallete.GetHexColor("Green"));
    }
}
