using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEnd : DialogueText
{
    public override void Activate(DialogueManager manager){
        manager.EndEvent();
    }
}
