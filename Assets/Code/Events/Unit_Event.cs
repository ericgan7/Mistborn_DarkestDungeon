using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Unit_Event : Unit, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] EventDialogue currentEvent;
    [SerializeField] Image highlight;
    [SerializeField] Team placeholderTeam;
    Vector2 defaultPos;

    void Awake(){
        gs = FindObjectOfType<GameState>();
        rt = GetComponent<RectTransform>();
        Moving = false;
        showUI = false;
        targetedState = TargetedState.Untargeted;
        defaultPos = rt.anchoredPosition;
        gameObject.SetActive(false);
        UnitTeam = placeholderTeam;
    }

    //when entering a room with attached event, it should call set event
    public void SetEvent(EventDialogue eventDialogue){
        currentEvent = eventDialogue;
        Character c = ScriptableObject.CreateInstance("Character") as Character;
        c.Init(eventDialogue.eventStats);
        SetCharacter(c, false);
        highlight.sprite = SpriteLibrary.GetCharSprite(c.GetSpriteHeader() + "_Highlight");
        highlight.enabled = false;
    }
    public override void OnPointerClick(PointerEventData pointer){
        if (currentEvent == null){
            return;
        }
        gs.dm.StartEvent(currentEvent);  
    }

    public override void OnPointerEnter(PointerEventData pointer){
        highlight.enabled = true;
    }

    public override void OnPointerExit(PointerEventData p){
        highlight.enabled = false;
    }

    public void EndEvent(){
        currentEvent = null;
    }

    public override void ResetPosition(){
        TargetPosition = defaultPos;
        velocity = Vector3.zero;
        scalev = Vector3.zero;
        targetScale = Vector3.one;
        Moving = true;
        elapsed = 0f;
        _time = moveTime;
    }
}
