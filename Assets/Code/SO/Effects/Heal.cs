﻿using System.Collections;
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
            target = target,
            amount = amount,
            result = Result.heal
        };
        results.targets.Add(r);
        target.stats.Heal(amount);
    }
}
