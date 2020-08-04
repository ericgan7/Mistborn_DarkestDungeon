using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stress : Effect
{
    public override EffectType Type => EffectType.stress;
    public override bool IsStress => true;
    int stressAmount;

    public Stress(int amount){
        stressAmount = amount;
    }

    public override int GetAmount(){
        return stressAmount;
    }

    public override void ApplyEffect(Unit actor, Unit target, ref AbilityResultList results)
    {
        DelayedAbilityResult r = new DelayedAbilityResult()
        {
            target = target,
            delayedEffect = this,
            result = DelayedResult.hit,
        };
        results.stress.Add(r);
    }

    public override void ApplyDelayedEffect(Unit actor, Unit target){
        target.stats.StressDamage(StressDamage(target));
    }
    public int StressDamage(Unit target){
        //TODO consider moving the extra stress damage into stats.stressdamage
        return stressAmount;
    }

    public override string ToString(Unit target){
        int amount = StressDamage(target);
        return string.Format("{0} {1} <color={2}></b>Stress<b></color>\n", 
            amount >= 0 ? "Deal" : "Recover", amount, ColorPallete.GetHexColor("Purple"));
    }
}
