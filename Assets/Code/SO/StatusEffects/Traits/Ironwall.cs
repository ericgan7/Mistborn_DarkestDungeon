using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ironwall : Traits
{
    public override string traitName => "Ironwall";
    public Ironwall(){}

    public override void ModifyAttack(Ability_Attack a, Unit actor, Unit target, ref AbilityResultList results){
        if (actor.stats.Defense().x > 0){
            a.ApplyCounter(actor, target, ref results);
        }
    }

    public override string ToString(){
        return string.Format("<color={0}><b>Ironwall</b></color>: Unit will always counter attacks when defense is above 0.\n",
        ColorPallete.GetHexColor("Blue"));
    }
}
