using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] DetailedCharacterMenu characterMenu;
    [SerializeField] CharacterDescription descriptionMenu;
    [SerializeField] CharacterSelectionLight spotlight;
    [SerializeField] RectTransform spotLightParent;
    [SerializeField] public GraphicRaycaster raycaster;

    [SerializeField] HeroRosterSlot slotPrefab;
    [SerializeField] RectTransform slotParent;

    [SerializeField] UnitSelection selectionPrefab;
    [SerializeField] RectTransform selectionParent;

    [SerializeField] List<TeamRosterSlot> teamSlots;

    [SerializeField] RectTransform start;
    [SerializeField] RectTransform center;
    UnitSelection selectedUnit;

    GameState state;
    Dictionary<Character, HeroRosterSlot> slots = new Dictionary<Character, HeroRosterSlot>();
    Dictionary<Character, UnitSelection> units = new Dictionary<Character, UnitSelection>();

    Character currentCharacter;
    Character highlightedCharacter;

    public void Awake(){
        state = FindObjectOfType<GameState>();
    }

    public void Start(){
        List<Character> roster = state.hr.GetRosterCharacters();
        for (int i = 0; i < roster.Count; ++i){
            HeroRosterSlot slot = Instantiate<HeroRosterSlot>(slotPrefab, slotParent.transform);
            slot.Init(roster[i], this);
            slots.Add(roster[i], slot);
            UnitSelection selection = Instantiate<UnitSelection>(selectionPrefab, selectionParent.transform);
            Vector2 position = start.anchoredPosition;
            position.x += i * 200;
            selection.SetPositionImmediate(position);
            selection.Init(roster[i], this);
            units.Add(roster[i], selection);
        }
    }

    public void StartMission(){
        SceneManager.LoadSceneAsync("1_IntroScene");
    }

    public void ChangeCharacterMenu(){
        characterMenu.Activate(!characterMenu.isActiveAndEnabled);
    }

    public void AddTeamCharacter(Character character, CharacterObj obj){
        foreach(TeamRosterSlot slot in teamSlots){
            if (slot.GetCharacter() == character){
                return;
            }
        }
        foreach(TeamRosterSlot slot in teamSlots){
            if (slot.GetCharacter() == null){
                slot.AssignCharacter(obj);
            }
        }
    }

    public void SetTeamCharacter(Character character, bool isSelected){
        units[character].SetTeamHighlight(isSelected);
    }

    public void MoveLight(Vector2 position){
        if (spotlight.IsOn()){
            spotlight.SetPositionMoving(position);
        } else {
            spotlight.SetOn(true);
            spotlight.SetPositionImmediate(position);
        }
    }

    public void MoveLight(RectTransform parent){
        if (spotlight.IsOn()){
            spotlight.transform.SetParent(parent.transform);
            spotlight.SetPositionMoving(Vector2.zero);
        } else {
            spotlight.transform.SetParent(parent.transform);
            spotlight.SetOn(true);
            spotlight.SetPositionImmediate(Vector2.zero);
        }
    }

    public void TurnLight(bool isOn){
        spotlight.SetOn(isOn);
    }

    void Deselect(){
        if (currentCharacter != null){
            //fade in characters
            foreach(UnitSelection u in units.Values){
                u.SetAlphaMoving(1.0f);
                u.SetLightMoving(0.5f);
                u.SetRaycast(true);
            }
            //move unit back to origin
            selectedUnit.SetPositionMoving(selectedUnit.Origin());
            //set slots and unit ui
            units[currentCharacter].Select(false);
            slots[currentCharacter].Select(false);
            currentCharacter = null;
        }
        if (highlightedCharacter != null){
            units[highlightedCharacter].Select(false);
            slots[highlightedCharacter].Select(false);
            highlightedCharacter = null;
        }
        descriptionMenu.gameObject.SetActive(false);
    }

    void Select(){
        if (currentCharacter == null){
            units[highlightedCharacter].Select(true);
            slots[highlightedCharacter].Select(true);
            descriptionMenu.SetCharacter(highlightedCharacter);
            characterMenu.SetCharacter(highlightedCharacter);
        }
        else {
            units[currentCharacter].Select(true);
            slots[currentCharacter].Select(true);
            descriptionMenu.SetCharacter(currentCharacter);
            characterMenu.SetCharacter(currentCharacter);
            selectedUnit = units[currentCharacter];
            //fade out other units
            foreach(UnitSelection u in units.Values){
                if (u == selectedUnit){
                    continue;
                }
                u.SetAlphaMoving(0f);
                u.SetLightMoving(0f);
                u.SetRaycast(false);
            }
            //move unit to center
            selectedUnit.SetPositionMoving(center.anchoredPosition);
        }
        descriptionMenu.gameObject.SetActive(true);
    }

    public void SetCharacter(Character character){
        if (currentCharacter == character){
            Deselect();
            TurnLight(false);
        } else {
            Deselect();
            currentCharacter = character;
            Select();
        }

    }

    public void HighlightCharacter(Character character){
        if (currentCharacter != null){
            return;
        }
        highlightedCharacter = character;
        Select();
    }

    public void UnHighlightCharacter(){
        if (currentCharacter != null){
            return;
        }
        Deselect();
        highlightedCharacter = null;
        TurnLight(false);
    }

}
