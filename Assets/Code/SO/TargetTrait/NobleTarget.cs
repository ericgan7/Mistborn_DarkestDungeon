using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NobelTarget : TraitTarget
{
    public override EffectType Trait => EffectType.ska;
    
    public NobelTarget(int bonusDamage): base(bonusDamage) {}
}
