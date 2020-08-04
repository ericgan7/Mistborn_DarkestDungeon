using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class TraitDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] BasicTooltip tooltip;
    [SerializeField] Image icon;
    Traits currentTrait;

    public void SetTrait(Traits traits){
        if (traits == null){
            return;
        }
        currentTrait = traits;
        description.text = traits.traitName;
        tooltip.SetDescription(traits.ToString());
        //set Icon;
    }

    public void OnPointerEnter(PointerEventData pointer){
        tooltip.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData pointer){
        tooltip.gameObject.SetActive(false);
    }

}
