using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class StatusIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image icon;
    [SerializeField] GameObject tooltipPrefab;
    public StatusEffect effect;

    GameObject tooltip;

    public void OnPointerEnter(PointerEventData p)
    {
        tooltip = Instantiate(tooltipPrefab, gameObject.transform);
        TextMeshProUGUI text = tooltip.GetComponentInChildren<TextMeshProUGUI>();
        text.text = effect.Stats();
    }

    public void OnPointerExit(PointerEventData p)
    {
        Destroy(tooltip);
        tooltip = null;
    }
}
