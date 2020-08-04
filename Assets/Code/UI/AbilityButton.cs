using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AbilityButton : MonoBehaviour, 
IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    GameState state;
    Ability ability;
    Unit currentUnit;
    public Image pic;
    Image highlight;
    AbilityMenu manager;
    bool selected;
    bool usable;
    public Sprite defaultEmpty;

    [SerializeField]AbilityTooltip tooltip;
    [SerializeField]BasicTooltip unusableTooltip;
   
    void Awake()
    {
        highlight = gameObject.GetComponent<Image>();
        highlight.enabled = false;
        selected = false;
        usable = false;
        manager = GetComponentInParent<AbilityMenu>();
        state = FindObjectOfType<GameState>();
        GameEvents.current.onChangeMetal += SetUsable;
    }
        
    public void SetAbility(Ability a, Unit actor)
    {
        ability = a;
        currentUnit = actor;
        tooltip.SetAbility(a);
        if (a == null){
            pic.sprite = defaultEmpty;
            pic.color = ColorPallete.GetColor("Black");
            usable = false;
        } else {
            pic.sprite = SpriteLibrary.GetAbilitySprite(a.abilityName);
            SetUsable();
        }
    }

    void SetUsable(){
        if (ability == null){
            return;
        }
        bool position = ability.usable.IsValidRank(currentUnit, currentUnit);
        bool cost = GameState.Instance.metal.metalLevel >= ability.metalCost;
        bool ammo = GameState.Instance.inventory.HasItem(new ItemAmount(GameState.Instance.inventory.ammo, ability.ammoCost));
        if (position && cost && ammo)
        {
            usable = true;
            pic.color = Color.white;
            unusableTooltip.SetDescription("");
        } else {
            usable = false;
            pic.color = ColorPallete.GetColor("Grey");
            string description = "";
            if (!position){
                description += "Out of position\n";
            }
            if (!cost){
                description += "Low metal reserves\n";
            }
            if (!ammo){
                description += "Low Ammunition\n";
            }
            unusableTooltip.SetDescription(description);
        }
    } 

    public void DeselectAbility()
    {
        if (usable){
            highlight.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData p)
    {
        tooltip.gameObject.SetActive(true);
        if (unusableTooltip.HasText()){
            unusableTooltip.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData p)
    {
        tooltip.gameObject.SetActive(false);
        unusableTooltip.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData p){
        if (!usable){
            return;
        }
        if (manager.SelectAbility(this, ability)){
            highlight.enabled = true;
        } 
    }

}
