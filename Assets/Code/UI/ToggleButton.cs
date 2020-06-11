using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToggleButton : MonoBehaviour, IPointerClickHandler
{
    public string menuName;
    public GameObject menu;
    public Image highlight;
    public ToggleMenu group;

    public void OnPointerClick(PointerEventData pointer){
        group.SelectToggle(this);
    }

    public void Select(){
        highlight.color = Color.blue;
        highlight.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
        if (menu != null){
            menu.SetActive(true);
        }
    }

    public void Deselect(){
        highlight.color = Color.white;
        highlight.transform.localScale = Vector3.one;
        if (menu != null){
            menu.SetActive(false);
        }
    }
}
