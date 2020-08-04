using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Item Ability")]
public class Ability_Item : ScriptableObject
{
    public bool isAOE;

    public List<string> string_effects;
    List<Effect> effects;

    public List<Effect> Effects {get {if (effects == null)Init(); return effects; }}

    void Init(){
        if (effects == null){
            effects = new List<Effect>();
            foreach (string e in string_effects){
                effects.Add(EffectLibrary.GetEffect(e));
            }
        }
    }

    public void ApplyAbility(Unit actor, ref AbilityResultList results)
    {
        if (effects == null){
            Init();
        }
        if (isAOE){
            foreach (Unit u in actor.UnitTeam.GetUnits()){
                foreach (Effect e in effects){
                    e.ApplyEffect(u, u, ref results);
                }
            }
        } else {
            foreach (Effect e in effects){
                e.ApplyEffect(actor, actor, ref results);
            }
        }
    }
}
