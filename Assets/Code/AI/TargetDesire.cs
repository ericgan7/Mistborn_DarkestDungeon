using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Target Desire")]
public class TargetDesire : ScriptableObject
{
    public TargetDesireType type;
    public float weight;
}

public enum TargetDesireType
{
    health,
    will,
    bleedresist,
    stunresist,
    debuffresist,
    moveresist,
    random,
    marked,
    stunned,
}
