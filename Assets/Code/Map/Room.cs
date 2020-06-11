using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;
using UnityEngine.UI;

public enum RoomState{
    revealed,
    hidden,
    seen,
}
public class Room : MonoBehaviour, IPointerClickHandler
{
    public int id;
    public RectTransform rt;
    public Vector2Int coordinates;
    public MapController map;
    bool enterTrigger;
    bool exitTrigger;
    public Event enterEvent;
    public Event exitEvent;

    public RoomState state;
    public Image roomImage;
    public Sprite backgroundImage;
    public MapUnit currentUnit;

    public void Awake()
    {
        rt = GetComponent<RectTransform>();
        coordinates = new Vector2Int((int) rt.anchoredPosition.x / 100, (int) rt.anchoredPosition.y / 100);
        map = FindObjectOfType<MapController>();
        enterTrigger = true;
        exitTrigger = true;
        state = RoomState.hidden;
        roomImage = GetComponent<Image>();
        SetRoomImage();
        backgroundImage = SpriteLibrary.GetBackground("random");
    }

    public void SetPostion(Vector2Int coord){
        coordinates = coord;
        rt.anchoredPosition = new Vector2(coord.x * 100, coord.y * 100);
    }

    public bool IsNear(Room room){
        if (room.coordinates == coordinates){ //return false if they are the same room
            return false;
        }
        return MDistance(room) == 1;
    }

    public int MDistance(Room room){
        Vector2Int diff = room.coordinates - coordinates;
        return Mathf.Abs(diff.x) + Mathf.Abs(diff.y);
    }

    public void OnPointerClick(PointerEventData p)
    {
        Debug.Log("clicked on room " + id.ToString());
        map.MoveToRoom(this);
    }

    public void OnRoomEnter()
    {
        
    }

    public void AddTrap(Trap trap){
    }

    public void HideRoom(){
        if (state == RoomState.hidden){
            return;
        } else {
            state = RoomState.seen;
        }
        SetRoomImage();
    }

    public void SetRoom(RoomState newState){
        state = newState;
        SetRoomImage();
    }

    void SetRoomImage(){
        switch(state){
        case RoomState.hidden:
            roomImage.color = Color.black;
            break;
        case RoomState.revealed:
            roomImage.color = Color.white;
            break;
        case RoomState.seen:
            roomImage.color = Color.gray;
            break;
        }
    }

    public void CreateConnection(Room destination, UILineRenderer prefab, Transform parent)
    {
        UILineRenderer conn = Instantiate<UILineRenderer>(prefab, parent);
        conn.gameObject.SetActive(true);
        List<Vector2> points = new List<Vector2>() { rt.anchoredPosition };
        Debug.Log(rt.anchoredPosition - destination.rt.anchoredPosition);
        if (rt.anchoredPosition.x != destination.rt.anchoredPosition.x)
        {
            points.Add(new Vector2(destination.rt.anchoredPosition.x, rt.anchoredPosition.y));
        }
        if (rt.anchoredPosition.y != destination.rt.anchoredPosition.y)
        {
            points.Add(new Vector2(destination.rt.anchoredPosition.x, destination.rt.anchoredPosition.y));
        }
        conn.Points = points.ToArray();
    }
}