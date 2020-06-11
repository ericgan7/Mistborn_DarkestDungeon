using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suprise : StatBuff
{
    public Suprise():
        base("Speed", -10f, 1){
    }

    public override string ToString()
    {
        return string.Format("Suprised!: -{0} speed for {1} turns.", amount, duration);
    }
}
