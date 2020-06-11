using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reward : ScriptableObject
{
    public virtual bool IsItem {get {return false;}}
    public virtual bool IsCharacter {get {return false;}}
    public virtual bool IsState {get{return false;}}
    public virtual ItemInstance GetItemRewards(){
        return null;
    }
    public virtual void GetCharacterRewards(Unit unit){
        return;
    }

    public virtual void GetStateRewards(GameState state){
        return;
    }
}
