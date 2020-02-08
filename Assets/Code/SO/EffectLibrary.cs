using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class EffectLibrary
{
    public static Dictionary<string, Effect> effects;
    public static void Init()
    {
        effects = new Dictionary<string, Effect>();
    }

    public static Effect GetEffect(string param)
    {
        if (effects.ContainsKey(param))
        {
            return effects[param];
        }
        string[] p = param.Split(' ');
        switch (p[0])
        {
            case "Bleed":
                effects[param] = new Bleed(int.Parse(p[1]), int.Parse(p[2]));
                break;
            case "Counter":
                effects[param] = new Counter(int.Parse(p[1]), float.Parse(p[2]));
                break;
            case "Heal":
                effects[param] = new Heal(int.Parse(p[1]));
                break;
            case "Defend":
                effects[param] = new Defend(int.Parse(p[1]));
                break;
            case "StressHeal":
                effects[param] = new StressHeal(int.Parse(p[1]));
                break;
            case "Mark":
                effects[param] = new Mark(int.Parse(p[1]));
                break;
            case "Push":
            case "Pull":
                effects[param] = new PushPull(int.Parse(p[1]));
                break;
            case "Move":
                effects[param] = new Move();
                break;
            case "Dodge":
            case "Damage":
            case "Crit":
            case "Acc":
            case "Speed":
                effects[param] = new StatBuff(p[0], float.Parse(p[1]));
                break;
        }
        return effects[param];
    }
}

public class EffectParam
{
    public string key;
    public StatType statType;
    public float amount;
}
