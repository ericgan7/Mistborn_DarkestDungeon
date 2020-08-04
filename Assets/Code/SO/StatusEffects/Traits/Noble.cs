using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Noble : Traits
{
    public override EffectType Type => EffectType.noble;
    public override string traitName => "Noble";

    public override string ToString(){
        return "Unit is a noble.";
    }
}
