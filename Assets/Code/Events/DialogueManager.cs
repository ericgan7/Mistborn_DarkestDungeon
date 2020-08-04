using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] CharacterToggleGroup characterSelection;
    [SerializeField] Unit_Event eventUnit;
    [SerializeField] DialogueList text;
    
    Unit currentUnit;
    EventDialogue currentEvent;
    GameState state;

    //placeholder
    [SerializeField] EventDialogue eventDialogue;

    void Awake(){
        state = FindObjectOfType<GameState>();
        state.dm = this;
    }

    public void Start(){
        //StartEvent(eventDialogue);
        characterSelection.Init(state.ally);
        gameObject.SetActive(false);
    }

    //starting event should stop map controller from moving
        //combat should also stop map movement
    public void StartEvent(EventDialogue e) {
        currentEvent = e;
        gameObject.SetActive(true);
        characterSelection.Init(state.ally);
        text.ClearDialogue();
        foreach(string s in currentEvent.dialogueText){
            text.CreateDialogue(s);
        }
        foreach(EventChoice c in currentEvent.choices){
            text.CreateChoice(c);
        }
        text.NextLine();
    }

    public void SelectChoice(EventChoice choice, Item usage){
        EventOutcome outcome = choice.Outcome(currentUnit, usage, text);
        text.ClearChoices();
        DisplayOutcome(outcome);
        text.NextLine();
    }

    public void DisplayOutcome(EventOutcome outcome){
        text.CreateDialogue(outcome.description);
        if (outcome.IsItem){
            text.CreateReward(outcome.items);
        }
        if (outcome.IsEffect){
            text.CreateAbility(outcome.effects);
        }
        if (outcome.isEnd){
            text.CreateEnd();
        } 
    }

    //display ability also ends event
    public void DisplayAbility(Ability_Event ability){
        EndEvent();
        state.gc.PlayEvent(ability, eventUnit);
    }

    public void EndEvent(){
        gameObject.SetActive(false);
        eventUnit.EndEvent();
    }

    public void DisplayItem(){
        text.OpenItems();
    }

    public void SetCurrentUnit(Unit unit){
        currentUnit = unit;
        state.uic.SetCurrentUnit(unit);
        state.gc.currentUnit = unit;
    }
}
