using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ToggleButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI menuName;
    [SerializeField] private GameObject menu;
    [SerializeField] private Image highlight;
    public ToggleMenu group;

    public void OnPointerClick(PointerEventData pointer){
        group.SelectToggle(this);
    }

    public virtual void Select(){
        highlight.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
        if (menu != null){
            menu.SetActive(true);
        }
    }

    public virtual void Deselect(){
        highlight.transform.localScale = Vector3.one;
        if (menu != null){
            menu.SetActive(false);
        }
    }

    public string GetMenu(){
        return menuName.text;
    }
}
