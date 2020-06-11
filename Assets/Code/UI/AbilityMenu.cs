using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMenu : MonoBehaviour
{
    public List<AbilityButton> abilityButtons;

    public List<Ability> generalAbilities; //move and pass;
    public GameState state;
    AbilityButton selected;
    void Awake(){
        abilityButtons = new List<AbilityButton>(GetComponentsInChildren<AbilityButton>());
        state = FindObjectOfType<GameState>();
    }

    public bool SelectAbility(AbilityButton abilityButton, Ability ability){
        if (state.ic.GetBlocked()){
            return false;
        }
        foreach(AbilityButton ab in abilityButtons){
            if (abilityButton == ab){
                if (selected != abilityButton){
                    state.ic.SelectAbility(ability);
                    state.uic.SelectAbility(ability);
                    selected = abilityButton;
                }
                else {
                    ab.DeselectAbility();
                    state.ic.DeselectAbility();
                    state.uic.DeselectAbility();
                    selected = null;
                    return false;
                }
            }
            else {
                ab.DeselectAbility();
            }
        }
        return true;
    }    

    public void SetUnit(Unit unit){
        List<Ability> abilities = unit.stats.GetAbilities();
        for (int i = 0; i < abilities.Count; ++i)
        {
            abilityButtons[i].SetAbility(abilities[i], unit);
        }
        List<Ability> itemAbilities = unit.stats.GetItemAbilities();
        for (int i = 0; i < itemAbilities.Count; ++i){
            abilityButtons[abilities.Count + i].SetAbility(itemAbilities[i], unit);
        }
        for (int i = 0; i < generalAbilities.Count; ++i){
            abilityButtons[itemAbilities.Count + abilities.Count + i].SetAbility(generalAbilities[i], unit);
        }
        selected = null;
    }
}
