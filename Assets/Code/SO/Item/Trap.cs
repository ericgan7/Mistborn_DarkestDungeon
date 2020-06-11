using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Trap")]
public class Trap : ScriptableObject
{
    public List<string> trapEffects;
    public bool isAlly;

    public void TriggerTrap(GameState state){
        if (isAlly){
            foreach (Unit u in state.enemy.GetUnits()){
                foreach(string e in trapEffects){
                    EffectLibrary.GetEffect(e).ApplyDelayedEffect(null, u);
                }
            }
        } else {
            foreach (Unit u in state.ally.GetUnits()){
                foreach(string e in trapEffects){
                    EffectLibrary.GetEffect(e).ApplyDelayedEffect(null, u);
                }
            }
        }
    }
}
