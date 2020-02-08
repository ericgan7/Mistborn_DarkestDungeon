using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
    IPointerClickHandler
{
    public Image icon;
    public TextMeshProUGUI count;
    public ItemTray inventory;

    public void Awake()
    {
        icon = GetComponent<Image>();
        count = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void AssignItem(ItemInstance item)
    {
        icon.sprite = item.item.icon;
        count.text = item.amount.ToString();
    }

    public void OnPointerClick(PointerEventData p)
    {

    }

    public void OnPointerEnter(PointerEventData p)
    {

    }

    public void OnPointerExit(PointerEventData p)
    {

    }
}
