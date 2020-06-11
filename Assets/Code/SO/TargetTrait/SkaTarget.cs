using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkaTarget : TraitTarget
{
    public override EffectType Trait => EffectType.ska;
    
    public SkaTarget(int bonusDamage): base(bonusDamage) {}
}
