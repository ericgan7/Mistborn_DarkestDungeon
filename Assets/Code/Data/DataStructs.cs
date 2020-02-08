using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompareUnitSpeed: IComparer<Unit>
{
    public int Compare(Unit a, Unit b)
    {
        if (a.stats.Speed() < b.stats.Speed())
        {
            return -1;
        }
        else if (a.stats.Speed() > b.stats.Speed())
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
    hit,
    miss,
    graze,
    dodge,
    heal,
    def,
    buff,
    crit,
    defcrit,
}

public enum StatType
{
    health,
    defense,
    will,
    damage,
    crit,
    acc,
    speed,
    dodge
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
            default:
                return StatType.dodge;
        }
    }
}

public enum ItemType
{
    potion,
    gold,
}