using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAbilty : DialogueText
{
    Ability_Event ability;

    public void SetEvent(Ability_Event ae){
        ability = ae;
    }
    public override void Activate(DialogueManager manager){
        gameObject.SetActive(true);
        manager.DisplayAbility(ability);
    }
}
