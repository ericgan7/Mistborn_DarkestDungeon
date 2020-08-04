using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/List of Target Preferences")]
public class AIPreferences : ScriptableObject
{
    public List<TargetDesire> targetDesires;

    public float GetDesire(Unit target)
    {
        float preference = 0f;
        foreach(TargetDesire d in targetDesires)
        {
            preference += HandleTargetDesire(d, target);
        }
        return preference;
    }

    public float HandleTargetDesire(TargetDesire desire, Unit target)
    {
        float preference = 0f;
        switch (desire.type)
        {
            case TargetDesireType.health:
                Vector2 hp = target.stats.Health();
                preference = 1f - (float)(hp.x / hp.y);
                break;
            case TargetDesireType.marked:
                preference =  target.stats.modifiers.IsMarked ? 1f : 0f;
                break;
            case TargetDesireType.stunned:
                preference =  target.stats.modifiers.IsStunned ? 1f : 0f;
                break;
            case TargetDesireType.bleedresist:
            case TargetDesireType.debuffresist:
            case TargetDesireType.moveresist:
            case TargetDesireType.stunresist:
                preference =  (float)(100 - target.stats.GetStat(TargetTypeToStatType(desire.type))) / 100f;
                break;
            case TargetDesireType.random:
            default:
                return 0f;
        }
        return preference * desire.weight;
    }

    public static StatType TargetTypeToStatType(TargetDesireType type)
    {
        switch (type)
        {
            case TargetDesireType.bleedresist:
                return StatType.bleedResist;
            case TargetDesireType.debuffresist:
                return StatType.debuffResist;
            case TargetDesireType.moveresist:
                return StatType.moveResist;
            case TargetDesireType.stunresist:
                return StatType.stunResist;
        }
        return StatType.health;
    }
}

