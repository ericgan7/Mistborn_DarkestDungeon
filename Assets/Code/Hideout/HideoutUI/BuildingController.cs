using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public GameObject missionScreen;
    public HideoutRoom activeRoom;

    public Vector3 buildingOffset;
    Vector3 cameraPosition;
    Vector3 defaultPosition;
    Vector3 cameraVelocity;
    float zoomTarget;
    float zoomVelocity;
    public float cameraTime;
    void Awake(){
        cameraPosition = Camera.main.transform.position;
        defaultPosition = cameraPosition;
        cameraVelocity = Vector3.zero;
        zoomTarget = 5;
        zoomVelocity = 0f;
    }

    public void SelectBuilding(HideoutRoom type){
        activeRoom = type;
        missionScreen.SetActive(true);//placeholder
        cameraVelocity = Vector3.zero;
        zoomVelocity = 0f;
        cameraPosition = type.transform.position + buildingOffset;
        zoomTarget = 3;
    }

    public void DeselectBuildling(){
        activeRoom.Reset();
        missionScreen.SetActive(false);//placeholder
        zoomTarget = 5;
        cameraPosition = defaultPosition;
        zoomVelocity = 0f;
        cameraVelocity = Vector3.zero;
        
    }

    void Update(){
        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, cameraPosition, ref cameraVelocity, cameraTime);
        Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, zoomTarget, ref zoomVelocity, cameraTime);
    }
}
