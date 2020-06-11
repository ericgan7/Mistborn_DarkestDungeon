using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterObj : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image icon;
    public GraphicRaycaster raycaster;
    public RectTransform rt;
    bool isDragging;
    public HeroRosterSlot slot;

    public HeroRosterSlot original;
    public Character character;

    void Update(){
        if (isDragging){
            rt.position = Input.mousePosition;
        }
    }
    public void SetDragging(bool drag){
        isDragging = drag;
    }
    public void OnPointerDown(PointerEventData p){
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData p){
        isDragging = false;
        CheckForSlot(p);
    }

    public void CheckForSlot(PointerEventData p){
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(p, results);
        foreach (RaycastResult r in results){
            HeroRosterSlot newSlot = r.gameObject.GetComponent<HeroRosterSlot>();
            if (newSlot != null && newSlot != slot){
                if (newSlot.AssignCharacter(this)){
                    slot.AssignCharacter(newSlot.draggable);
                    slot = newSlot;
                    icon.raycastTarget = true;
                    return;
                }
            }
        }
        rt.transform.parent = original.transform;
        rt.anchoredPosition = Vector3.zero;
    }

    public Character GetCharacter(){
        return character;
    }

    public void SetCharacter(Character c){
        character = c;
        icon.sprite = c.portrait;
    }

    public void Reset(){
        if (slot == null){
            Debug.Log("ERROR SLOT IS NULL");
        }
        rt.anchoredPosition = Vector3.zero;
    }
    
}
