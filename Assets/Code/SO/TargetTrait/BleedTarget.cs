using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedTarget : TraitTarget
{
    public override EffectType Trait => EffectType.bleed;
    
    public BleedTarget(int bonusDamage): base(bonusDamage) {}
}
