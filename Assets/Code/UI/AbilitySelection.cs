using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class AbilitySelection : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] DetailedCharacterMenu parent;
    Ability currentAbility;
    public Image icon;
    public Image highlight;
    [SerializeField]AbilityTooltip tooltip;
    bool selected;
    [SerializeField] bool isEquipment;

    public void SetAbility(Ability ability, bool isSelected){
        currentAbility = ability;
        if (ability == null){
            icon.sprite = null;
            icon.color = ColorPallete.GetColor("Grey");
            highlight.enabled = false;
        } else {
            icon.sprite = SpriteLibrary.GetAbilitySprite(ability.abilityName);
            icon.color = Color.white;
            tooltip.SetAbility(ability);
            tooltip.gameObject.SetActive(false);
            selected = isSelected;
            if (selected){
                highlight.enabled = true;
            }
            else {
                highlight.enabled = false;
            }
        }

    }

    public void OnPointerClick(PointerEventData pointer){
        if (currentAbility == null || isEquipment || parent == null){
            return;
        }
        if (selected){
            parent.DeselectSkill(currentAbility);
            highlight.enabled = false;
            selected = false;
        }else {
            if (parent.SelectSkill(currentAbility)){
                selected = true;
                highlight.enabled = true;
            }
        }
    }

    public void OnPointerEnter(PointerEventData pointer){
        if (currentAbility == null){
            return;
        }
        tooltip.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData pointer){
        if (currentAbility == null){
            return;
        }
        tooltip.gameObject.SetActive(false);
    }
}
