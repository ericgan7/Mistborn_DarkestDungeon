using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Event/Choice")]
public  class EventChoice : ScriptableObject
{
    public Item itemRequirement;
    //new class of stat check requirement
    public int chance;
    public string choiceDescription;
    public string choiceAction;
    public EventOutcome successOutcome;
    public EventOutcome failureOutcome;

    //checks character stats / items
        //determines outcome
        //prints out outcome to dialogue list
        //calls event outcome
    public EventOutcome Outcome(Unit actor, Item usage, DialogueList output){
        output.CreateDialogue(choiceAction);
        if (CheckSuccess(actor, usage)){
            return successOutcome;
        } else {
            return failureOutcome;
        }
    }

    bool CheckSuccess(Unit actor, Item usage){
        if (itemRequirement != null){
            return itemRequirement == usage;
        } else {
            int roll = Random.Range(0,100);
            return roll < chance;
        }
    }

    public virtual bool CheckUnitRequirements(Unit actor){
        return true;
    }

    //may need to check through type and amount
    public bool CheckItemRequirements(Item usage){
        return usage == itemRequirement;
    }
}
