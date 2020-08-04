using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroRosterSlot : MonoBehaviour, 
    IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    Character currentCharacter;
    [SerializeField] Image portrait;
    [SerializeField] Image border;
    [SerializeField] CharacterObj draggable;

    SelectionManager selectionManager;
    bool isAssigned = false;

    public Character GetCharacter(){
        return currentCharacter;
    }

    public void Init(Character character, SelectionManager manager){
        currentCharacter = character;
        draggable.Init(character, manager.raycaster, this);
        portrait.sprite = SpriteLibrary.GetPortrait(character.characterClass.className);
        selectionManager = manager;
        border.color = Color.grey;
    }

    public void Select(bool isSelected){
        if(isSelected){
            border.color = ColorPallete.GetColor("Orange");
        } else {
            border.color = Color.grey;
        }
    }
    
    public void Assign(bool assigned){
        isAssigned = assigned;
    }

    public void OnPointerClick(PointerEventData pointer){
        selectionManager.SetCharacter(currentCharacter);
        if (pointer.button == PointerEventData.InputButton.Right){
            selectionManager.ChangeCharacterMenu();
        }
    }

    public void OnPointerDown(PointerEventData pointer){
        if (draggable == null || pointer.button == PointerEventData.InputButton.Right || isAssigned){
            return;
        }
        draggable.SetDragging(true);
    }

    public void OnPointerUp(PointerEventData pointer){
        if (draggable == null || pointer.button == PointerEventData.InputButton.Right || isAssigned){
            return;
        }
        draggable.SetDragging(false);
        draggable.CheckForSlot(pointer);
    }
    
    public void OnPointerEnter(PointerEventData pointer){
        selectionManager.HighlightCharacter(currentCharacter);
    }

    public void OnPointerExit(PointerEventData pointer){
        selectionManager.UnHighlightCharacter();
    }
}
