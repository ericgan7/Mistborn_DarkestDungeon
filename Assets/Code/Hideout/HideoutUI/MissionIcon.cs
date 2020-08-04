using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MissionIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public List<RewardSlot> rewardSlots;
    public GameObject tooltip;
    public RectTransform rectTransform;

    public void Init(Mission mission){
        rectTransform.anchoredPosition = mission.position;
        for (int i = 0; i < rewardSlots.Count; ++i){
            //rewardSlots[i].SetItem(mission.rewards[i]);
        }
    }
    public void SetTooltip(bool active){
        tooltip.SetActive(active);
    }
    public void OnPointerEnter(PointerEventData pointer){
        tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData pointer){
        tooltip.SetActive(false);
    }
}
