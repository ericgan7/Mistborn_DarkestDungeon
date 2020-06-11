using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HideoutRoomType{
    missionSelect,
}
public class HideoutRoom : MonoBehaviour
{
    public HideoutRoomType type;
    BuildingController buildingController;
    BoxCollider2D detector;
    public void Awake(){
        buildingController = FindObjectOfType<BuildingController>();
        detector = GetComponent<BoxCollider2D>();
    }

    public void Reset(){
        detector.enabled = true;
    }
    void OnMouseUp(){
        Debug.Log("Select Room");
        detector.enabled = false;
        buildingController.SelectBuilding(this);
    }

    void OnMouseOver(){
        Debug.Log("Over Room");
    }
}
