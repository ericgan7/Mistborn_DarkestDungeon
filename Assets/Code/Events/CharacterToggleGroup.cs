using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterToggleGroup : ToggleMenu
{
    [SerializeField] DialogueManager manager;
    [SerializeField] Image characterImage;

    void Awake(){
        toggles = new List<ToggleButton>(GetComponentsInChildren<ToggleButton>());
        foreach(ToggleButton tb in toggles){
            tb.group = this;
        }
    }

    void Start(){}  //override menutoggle start

    public void Init(Team team){
        List<Unit> units = team.GetUnits();
        for (int i = 0; i < toggles.Count; ++i){
            if (i >= units.Count){
                ((CharacterToggle)toggles[i]).SetUnit(null);
            } else {
                ((CharacterToggle)toggles[i]).SetUnit(units[i]);
            }
        }
        toggles[0].Select();
    }

    public void SetCurrentUnit(Unit unit){
        if (unit == null){
            return;
        }
        characterImage.sprite = GetUnitSprite(unit);
        manager.SetCurrentUnit(unit);
    }

    Sprite GetUnitSprite(Unit unit){
        return SpriteLibrary.GetCharSprite(unit.stats.GetSpriteHeader() +"_Default");
    }

}
