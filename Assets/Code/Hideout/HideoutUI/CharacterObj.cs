using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterObj : MonoBehaviour
{
    [SerializeField] Image icon;
    GraphicRaycaster raycaster;
    [SerializeField] RectTransform rt;
    bool isDragging;
    HeroRosterSlot original;
    public TeamRosterSlot slot;
    Character currentCharacter;

    void Update(){
        if (isDragging){
            rt.position = Input.mousePosition;
        }
    }

    public void Init(Character character, GraphicRaycaster raycast, HeroRosterSlot origin){
        currentCharacter = character;
        raycaster = raycast;
        original = origin;
        icon.sprite = SpriteLibrary.GetPortrait(character.characterClass.className);
    }

    public Character GetCharacter(){
        return currentCharacter;
    }

    public void SetDragging(bool drag){
        isDragging = drag;
        gameObject.SetActive(true);
    }

    public void CheckForSlot(PointerEventData p){
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(p, results);
        bool assignment = false;
        foreach (RaycastResult r in results){
            TeamRosterSlot newSlot = r.gameObject.GetComponent<TeamRosterSlot>();
            if (newSlot != null){
                if (slot != null){
                    slot.AssignCharacter(null);
                }
                slot = newSlot;
                newSlot.AssignCharacter(this);
                transform.SetParent(newSlot.transform);
                assignment = true;
                original.Assign(true);
            }
        }
        if (!assignment){
            if (slot != null){
                slot.AssignCharacter(null);
            }
        }
        rt.anchoredPosition = Vector3.zero;
    }

    public void Reset(){
        original.Assign(false);
        rt.SetParent(original.transform);
        rt.anchoredPosition = Vector2.zero;
        slot = null;
    }
    
}
