using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TeamRosterSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public CharacterObj currentCharacter;
    SelectionManager manager;

    void Awake(){
        manager = FindObjectOfType<SelectionManager>();
    }

    public Character GetCharacter(){
        if (currentCharacter){
            return currentCharacter.GetCharacter();
        }
        return null;
    }

    public void AssignCharacter(CharacterObj obj){
        //if passed in a null, we are reseting current character
        //else we are changing character.
        if (currentCharacter != null){
            currentCharacter.Reset();
            manager.SetTeamCharacter(currentCharacter.GetCharacter(), false);
        }
        currentCharacter = obj;
        if (currentCharacter != null){
            manager.SetTeamCharacter(currentCharacter.GetCharacter(), true);
        }
    }

    public void OnPointerClick(PointerEventData pointer){
        if (pointer.button == PointerEventData.InputButton.Right){
            if (currentCharacter){
                AssignCharacter(null);
            }
        }
    }

    public void OnPointerDown(PointerEventData pointer){
        if (currentCharacter == null || pointer.button == PointerEventData.InputButton.Right){
            return;
        }
        currentCharacter.SetDragging(true);
    }

    public void OnPointerUp(PointerEventData pointer){
        if (currentCharacter == null || pointer.button == PointerEventData.InputButton.Right){
            return;
        }
        currentCharacter.SetDragging(false);
        currentCharacter.CheckForSlot(pointer);
    }
}
