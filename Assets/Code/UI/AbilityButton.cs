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
    public Ability ability;
    public Image pic;
    Image highlight;
    AbilityMenu manager;
    bool selected;
    bool usable;
    public Sprite defaultEmpty;

    [SerializeField]Tooltip tooltip;
   
    void Awake()
    {
        highlight = gameObject.GetComponent<Image>();
        highlight.enabled = false;
        selected = false;
        usable = false;
        manager = GetComponentInParent<AbilityMenu>();
        state = FindObjectOfType<GameState>();
    }
        
    public void SetAbility(Ability a, Unit actor)
    {
        ability = a;
        tooltip.SetAbility(a);
        if (a == null){
            pic.sprite = defaultEmpty;
            pic.color = ColorPallete.GetColor("Black");
            usable = false;
        } else {
            pic.sprite = SpriteLibrary.GetAbilitySprite(a.abilityName);
            if (a.usable.IsValidRank(actor, actor))
            {
                //low metal should have debuffed version;
                //if (state.am.metalLevel >= a.metalCost){
                    usable = true;
                    pic.color = Color.white;
                //}
            } else {
                usable = false;
                pic.color = ColorPallete.GetColor("Grey");
            }
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
    }

    public void OnPointerExit(PointerEventData p)
    {
        tooltip.gameObject.SetActive(false);
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
