using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AbilityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GameState state;
    Ability ability;
    public Image pic;
    public Toggle toggle;
    public EventSystem es;
    Button button;
    public int index { get; set; }
    bool selected;
    bool usable;

    [SerializeField] public GameObject tooltipPrefab;
    GameObject tooltip;
   
    void Awake()
    {
        state = FindObjectOfType<GameState>();
        pic = gameObject.GetComponent<Image>();
        button = GetComponent<Button>();
        selected = false;
        usable = false;
        es = EventSystem.current;
    }
        
    public void SetAbility(Ability a, Unit actor)
    {
        ability = a;
        pic.sprite = a.icon;
        Debug.Log(a.abilityName);
        if (a.usable.IsValidRank(actor, actor))
        {
            usable = true;
        } else
        {
            usable = false;
        }
        toggle.interactable = usable;
    } 

    public void SelectAbility()
    {
        Debug.Log(toggle.spriteState);
        if (state.ic.GetBlocked() || !usable)
        {
            return;
        }
        if (!selected)
        {
            state.ic.SelectAbility(ability);
            state.uic.SelectAbility(ability);
            //pic.color = Color.red;
            selected = true;
            
        } else
        {
            es.SetSelectedGameObject(null);
            state.ic.DeselectAbility();
            state.uic.DeselectAbility();
            //pic.color = Color.white;
            selected = false;
        }
    }

    public void DeselectAbility(int i)
    {
        if (index != i)
        {
            pic.color = Color.white;
        }
    }

    public void OnPointerEnter(PointerEventData p)
    {
        tooltip = Instantiate(tooltipPrefab, gameObject.transform);
        Tooltip text = tooltip.GetComponentInChildren<Tooltip>();
        text.SetAbility(ability);
    }

    public void OnPointerExit(PointerEventData p)
    {
        Destroy(tooltip);
        tooltip = null;
    }

}
