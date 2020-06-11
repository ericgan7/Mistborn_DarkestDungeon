using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroRosterSlot : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public DetailedCharacterMenu menu;
    public Image portrait;
    public Character currentCharacter;
    public CharacterObj draggable;
    public bool rosterSlot;
    public Character GetCharacter(){
        return currentCharacter;
    }
    public void Init(Character character){
        currentCharacter = character;
        rosterSlot = true;
        draggable = GetComponentInChildren<CharacterObj>(true);
        draggable.gameObject.SetActive(true);
        draggable.SetCharacter(character);
        draggable.original = this;
        draggable.slot = this;
        //set portrait
    }

    public bool AssignCharacter(CharacterObj obj){
        if (rosterSlot){
            return false;
        }
        if (obj == null){
            return false;
        }
        currentCharacter = obj.GetCharacter();
        draggable = obj;
        obj.slot = this;
        obj.transform.parent = transform;
        obj.rt.anchoredPosition = Vector3.zero;
        return true;
    }
    public void SetRaycaster(GraphicRaycaster raycaster){
        draggable.raycaster = raycaster;
    }
    public void OnPointerClick(PointerEventData pointer){
        if (pointer.button == PointerEventData.InputButton.Right){
            //open character screen for level up or dialogue;
            Debug.Log("Click Hero Roster Button ");
            menu.SetCharacter(currentCharacter);
            menu.gameObject.SetActive(true);
        }
    }

    public void OnPointerDown(PointerEventData pointer){
        if (draggable == null){
            return;
        }
        draggable.icon.raycastTarget = false;
        draggable.SetDragging(true);
    }

    public void OnPointerUp(PointerEventData pointer){
        if (draggable == null){
            return;
        }
        draggable.SetDragging(false);
        draggable.CheckForSlot(pointer);
    }
    
}
