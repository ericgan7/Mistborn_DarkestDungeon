using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName= "Ability/ParticleEffect")]
public class AbilityParticleEffect : ScriptableObject
{
    public Color color;
    public Sprite particle;
    public float direction;
}
