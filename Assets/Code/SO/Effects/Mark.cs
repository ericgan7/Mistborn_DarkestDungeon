using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : StatusEffect
{
    public override bool IsMark => true;

    public Mark(int d)
    {
        duration = d;
    }
}
