using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MetalManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int metalLevel;
    public int max_metal;
    static Dictionary<int, string> metalString = new Dictionary<int, string>(){
        {0, "Empty"},
        {1, "Low"},
        {2, "Normal"},
        {3, "High"}
    };
    [SerializeField] PipBar metalBar;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] BasicTooltip tooltip;
    RectTransform tooltipRt;

    public List<EnvironmentBuffs> metalBuffs;

    public void Awake(){
        GameState.Instance.metal = this;
        tooltipRt = tooltip.GetComponent<RectTransform>();
        metalBar.Init(metalLevel, max_metal);
        UpdateMetal(metalLevel);
        description.transform.SetAsLastSibling();
    }

    public void UpdateMetal(int change)
    {
        metalLevel = Mathf.Clamp(metalLevel + change, 0, max_metal);
        metalBar.SetAmount(metalLevel);
        description.text = metalString[metalLevel];
        GameEvents.current.MetalTrigger();
    }

    public EnvironmentBuffs GetMetalEffects(){
        foreach(EnvironmentBuffs buffs in metalBuffs){
            if (metalLevel >= buffs.level){
                return buffs;
            }
        }
        return null;
    }

    public void OnPointerEnter(PointerEventData pointer){
        tooltip.SetDescription(metalBuffs[metalLevel].ToString());
        tooltip.gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipRt);
    }

    public void OnPointerExit(PointerEventData pointer){
        tooltip.gameObject.SetActive(false);
    }
}
