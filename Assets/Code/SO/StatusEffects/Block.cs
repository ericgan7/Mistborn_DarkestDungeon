using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : StatusEffect
{
    int amount;
    public override EffectType Type => EffectType.block;
    public Block(int a, int d)
    {
        amount = a;
        duration = d;
    }

    public bool DecreaseAmount()
    {
        Debug.Log("USED BLOCK");
        --amount;
        return amount == 0;
    }

    public override string ToString()
    {
        string block = amount == 1 ? "attack" : string.Format("{0} attacks", amount);
        return string.Format("<color={2}><b>Block</b></color> the next {0} for {1} turns", 
            block, duration, ColorPallete.GetHexColor("Blue"));
    }
}
