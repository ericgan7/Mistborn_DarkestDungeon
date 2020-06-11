using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatBuff : StatusEffect
{
    protected float amount;
    protected StatType stat;
    public override bool IsStatChange => true;
    public override EffectType Type => amount < 0 ? EffectType.debuff : EffectType.buff;
    public override bool IsDebuff => amount < 0 ? false : true;

    public StatBuff(string s, float a, int d)
    {
        stat = StatName.ToStat(s);
        amount = a;
        duration = d;
    }
    public override float GetStat(StatType type, Unit actor = null, Unit target = null)
    {
        if (type == stat)
        {
            return amount;
        }
        return 0f;
    }

    public override string ToString()
    {
        if (duration < 0){
            return string.Format("{0} {1}\n", amount, StatName.ToString(stat));
        }
        return string.Format("{0}{1} <color={4}>{2}</color> for {3} turns\n", 
            amount < 0 ? "" : "+", amount, StatName.ToString(stat), duration, ColorPallete.GetStatHexColor(stat));
    }

    public override string ToString(Unit target){
        if (amount > 0 && target.stats.modifiers.CanBuff){
            return ToString();
        } else {
            return string.Format("<color={1}><b>Debuff</b></color>: {0}% chance\n", 
                100 - target.stats.GetStat(StatType.debuffResist), ColorPallete.GetHexColor("Orange"));
        }
    }
}
