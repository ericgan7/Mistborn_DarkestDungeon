using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MetalVialToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AlarmManager manager;
    public GameObject prefab;
    GameObject tooltip;
    
    GameState state;

    public void Awake(){
        state = FindObjectOfType<GameState>();
    }
    public void OnPointerEnter(PointerEventData pointer){
        tooltip = Instantiate<GameObject> (prefab, transform);
        TextMeshProUGUI text = tooltip.GetComponentInChildren<TextMeshProUGUI>();
        EnvironmentBuffs buffs = manager.GetMetalEffects();
        text.text = buffs.GetString();
    }

    public void OnPointerExit(PointerEventData pointer){
        Destroy(tooltip);
    }
}
