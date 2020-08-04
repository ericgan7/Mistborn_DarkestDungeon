using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompareUnitSpeed: IComparer<Unit>
{
    public int Compare(Unit a, Unit b)
    {
        int aspeed = a.stats.GetStat(StatType.speed);
        int bspeed = b.stats.GetStat(StatType.speed);
        if (aspeed < bspeed)
        {
            return -1;
        }
        else if (aspeed > bspeed)
        {
            return 1;
        }
        else
        {
            return 0;   //can randomize order if equal.
        }
    }
}

public enum TargetedState
{
    Untargeted,
    Targeted,
    Current,
}

public enum Result
{
    Hit,
    Miss,
    Graze,
    Dodge,
    Block,
    Heal,
    Def,
    Buff,
    Crit,
    DefCrit,
    Stress,
    StressHeal,
    none,
}

public enum DelayedResult
{
    hit,
    resist
}

public enum StatType
{
    none,
    health,
    defense,
    will,
    damage,
    crit,
    acc,
    speed,
    dodge,
    bleedResist,
    moveResist,
    stunResist,
    debuffResist,
    stressResist,
}

public enum EffectType
{
    //abilities
    attack,
    crit,
    buff,
    debuff,
    bleed,
    stun,
    none,
    mark,
    block,
    move,
    guard,
    stress,
    //traits
    ska,
    noble,
    baseClass,
}

public static class EffectName
{
    public static string ToString(EffectType type){
        switch (type){
            case EffectType.buff:
                return "Buff";
            case EffectType.debuff:
                return "Debuff";
            case EffectType.bleed:
                return "Bleed";
            case EffectType.stun:
                return "Stun";
            case EffectType.mark:
                return "Mark";
            case EffectType.block:
                return "Block";
        }
        return "";
    }
}

public static class StatName
{
    public static string ToString(StatType type)
    {
        switch (type)
        {
            case StatType.health:
                return "Health";
            case StatType.defense:
                return "Defense";
            case StatType.will:
                return "Will";
            case StatType.damage:
                return "Damage";
            case StatType.crit:
                return "Crit";
            case StatType.acc:
                return "Acc";
            case StatType.speed:
                return "Speed";
            case StatType.dodge:
                return "Dodge";
            case StatType.stressResist:
                return "Stress Resist";
        }
        return "";
    }

    public static StatType ToStat(string s)
    {
        switch (s)
        {
            case "Health":
                return StatType.health;
            case "Defense":
                return StatType.defense;
            case "Will":
                return StatType.will;
            case "Damage":
                return StatType.damage;
            case "Crit":
                return StatType.crit;
            case "Acc":
                return StatType.acc;
            case "Speed":
                return StatType.speed;
            case "Dodge":
                return StatType.dodge;
            case "StressResist":
                return StatType.stressResist;
        }
        return StatType.none;
    }
}

public enum GameMode {
    exploration,
    combat,
}