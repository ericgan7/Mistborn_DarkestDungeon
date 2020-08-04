using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class WeaponSelection : MonoBehaviour, 
IPointerClickHandler, IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] DetailedCharacterMenu menu;
    [SerializeField] CharacterDescription description;
    [SerializeField] Image weaponIcon;
    [SerializeField] Image highlight;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] BasicTooltip tooltip;

    Weapon currentWeapon;

    public bool isWeapon(Weapon weapon){
        return weapon.weaponName == currentWeapon.weaponName;
    }

    public void SetWeaponImage(Weapon weapon){
        weaponIcon.sprite = weapon.weaponIcon;
        highlight.enabled = false;
        currentWeapon = weapon;
        tooltip.SetWeapon(weapon);
    }

    public void SetWeaponText(Weapon weapon){
        title.text = weapon.weaponName;
        currentWeapon = weapon;
    }

    public void SelectWeapon(){
        highlight.enabled = true;
    }

    public void DeselectWeapon(){
        highlight.enabled = false;
    }

    public void OnPointerClick(PointerEventData pointer){
        if (menu != null){
            menu.SelectWeapon(currentWeapon);
        }
        if (description != null){
            description.SelectWeapon(currentWeapon);
        }
        SelectWeapon();
    }

    public void OnPointerEnter(PointerEventData pointer){
        if (tooltip){
            tooltip.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData pointer){
        if (tooltip){
            tooltip.gameObject.SetActive(false);
        }
    }
}
