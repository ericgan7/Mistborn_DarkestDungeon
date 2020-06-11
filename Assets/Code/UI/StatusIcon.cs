using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class StatusIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public RectTransform rt;
    Vector2 offset;
    [SerializeField] GameObject tooltip;
    [SerializeField] TextMeshProUGUI text;

    public EffectType type;
    public StatusEffectManager manager;

    public void Awake()
    {
        Vector2 offset = rt.anchoredPosition;
        tooltip.SetActive(false);
    }
    public void SetImage(Sprite s){
        icon.sprite = s;
    }
    public void SetPosition(int index)
    {
        rt.anchoredPosition = new Vector2(offset.x + rt.sizeDelta.x * index, offset.y);
    }

    public void OnPointerEnter(PointerEventData p)
    {
        tooltip.SetActive(true);
        string description = "";
        switch (type)
        {
            case EffectType.bleed:
                foreach (StatusEffect e in manager.Bleed)
                {
                    description += e.ToString();
                }
                break;
            case EffectType.block:
                description = manager.Block.ToString();
                break;
            case EffectType.buff:
                foreach (StatusEffect e in manager.Modifiers)
                {
                    if (e.Type == EffectType.buff)
                    {
                        description += e.ToString();
                    }
                }
                break;
            case EffectType.debuff:
                foreach (StatusEffect e in manager.Modifiers)
                {
                    if (e.Type == EffectType.debuff)
                    {
                        description += e.ToString();
                    }
                }
                break;
            case EffectType.mark:
                description = manager.Mark.ToString();
                break;
            case EffectType.stun:
                description = manager.Stun.ToString();
                break;
            case EffectType.baseClass:
                foreach(Traits t in manager.Traits){
                    description += t.ToString();
                }
                break;
        }
        text.text = description;
    }

    public void OnPointerExit(PointerEventData p)
    {
        tooltip.SetActive(false);
    }
}
