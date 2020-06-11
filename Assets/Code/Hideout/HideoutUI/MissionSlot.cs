using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MissionSlot : MonoBehaviour, IPointerClickHandler
{
    public MissionUI manager;
    public TextMeshProUGUI type;
    public TextMeshProUGUI district;
    public TextMeshProUGUI difficulty;
    public MissionIcon icon;
    public Mission mission;

    public void SetMission(Mission m){
        type.text = MissionString.MissionTypeString(m.type);
        district.text = MissionString.DistrictString(m.district);
        difficulty.text = m.difficulty.ToString();
        mission = m;
    }

    public void OnPointerClick(PointerEventData pointer){
        manager.SelectMission(this);
    }
}
