using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metal : Effect
{
    int amount;
    public Metal(int a){
        amount = a;
    }

    public override void ApplyEffect(Unit actor, Unit target, ref AbilityResultList results){
        GameState.Instance.metal.UpdateMetal(amount);
    }

    public override string ToString(){
        return string.Format("+{0} to <color={1}>Metal</color> reserves", amount, ColorPallete.GetHexColor("Yellow"));
    }
}
