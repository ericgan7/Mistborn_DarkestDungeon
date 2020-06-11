using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunTarget : TraitTarget
{
    public override EffectType Trait => EffectType.stun;
    
    public StunTarget(int bonusDamage): base(bonusDamage) {}
}
