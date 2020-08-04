using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueItem : DialogueText
{
    public override void Activate(DialogueManager manager){
        manager.DisplayItem();
    }
}
