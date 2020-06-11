using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkTarget : TraitTarget
{
    public override EffectType Trait => EffectType.mark;
    
    public MarkTarget(int bonusDamage): base(bonusDamage) {}
}
