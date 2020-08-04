using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : StatusEffect
{
    public override bool IsMark => true;
    public override EffectType Type => EffectType.mark;
    public override bool IsPermanant => false;

    public Mark(int d)
    {
        duration = d;
    }

    public override string ToString(){
        return string.Format("<color={1}><b>Mark</b></color> for {0} turns", 
        duration, ColorPallete.GetHexColor("Red"));
    }
}
