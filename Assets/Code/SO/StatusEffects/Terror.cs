using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terror : StatusEffect
{
    int amount;
    public Terror(int d, int a){
        duration = d;
        amount = a;
    }
    public override int ApplyOverTime(Unit target){
        target.stats.StressDamage(amount);
        return amount;
    }
}
