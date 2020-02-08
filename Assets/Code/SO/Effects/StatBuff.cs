using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatBuff : StatusEffect
{
    float amount;
    StatType stat;
    public override bool IsStatChange => true;

    public StatBuff(string s, float a)
    {
        stat = StatName.ToStat(s);
        amount = a;
    }
    public override float GetStat(StatType type)
    {
        if (type == stat)
        {
            return amount;
        }
        return 0f;
    }

    public override string Stats()
    {
        return string.Format("{0} {1}\n", amount, StatName.ToString(stat));
    }
}
