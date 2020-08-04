using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterToggle : ToggleButton
{
    [SerializeField] Image background;
    [SerializeField] Image icon;
    [SerializeField] Image border;
    [SerializeField] Unit unit;

    public void SetUnit(Unit u){
        unit = u;
        if (unit == null){
            icon.sprite = null;
            icon.color = ColorPallete.GetColor("Grey");
            background.enabled = false;
            icon.enabled = false;
        } else {
            icon.enabled = true;
            background.enabled = true;
            icon.sprite = SpriteLibrary.GetStatusSprite(unit.stats.GetClassName());
            icon.color = Color.white;
        }
        border.enabled = false;
    }

    public override void Select(){
        border.enabled = true;
        ((CharacterToggleGroup) group).SetCurrentUnit(unit);
    }

    public override void Deselect(){
        border.enabled = false;
    }
    
}
