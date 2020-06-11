using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public enum Behavior{
    patrol, 
    hunt,
    idle,
    search
}
public class MapUnit : MonoBehaviour
{
    public Room location;
    public MapController map;
    RectTransform rt;
    Vector2 velocity;
    public float moveTime;
    public List<Vector2Int> route;
    public List<Vector2Int> waypoints;
    public EnemyGroup units;
    int wayPos;
    int routePos;
    bool moving;
    Behavior state;

    public int vision;
    public bool isPlayer;

    public UILineRenderer moveIntent;

    public void Awake()
    {
        rt = GetComponent<RectTransform>();
        velocity = Vector2.zero;
        state = Behavior.patrol;
        route = new List<Vector2Int>();
        map = FindObjectOfType<MapController>();
        moveIntent.gameObject.SetActive(false);
    }

    public void SetPosition(Vector2 position){
        rt.anchoredPosition = position;
    }

    public void SetPosition(Room room){
        rt.anchoredPosition = room.rt.anchoredPosition;
        location = room;
    }

    public void MoveToRoom(Room adjacentRoom)
    {
        location = adjacentRoom;
        velocity = Vector2.zero;
        moving = true;
    }

    public void SetRoute(List<Vector2Int> newwaypoints){
        // waypoints.Clear();
        // waypoints.Add( new Vector2Int((int) location.rt.anchoredPosition.x / 100, (int) location.rt.anchoredPosition.y / 100));
        // foreach(Vector2Int r in newRoute){
        //     waypoints.Add(r);
        // }
        waypoints = newwaypoints;
        route.Clear();
        routePos = 0;
        wayPos = 0;
    }
    
    //ai
    public void AdvanceTurn(){
        if(isPlayer){
            return;
        }
        if (state == Behavior.patrol){
            if (route.Count == 0){
                route = map.FindPath(location.coordinates, waypoints[routePos]);
                if (route == null){
                    Debug.Log("Could not patrol. Route empty");
                    return;
                }
            }
            if (routePos >= route.Count){
                wayPos = (wayPos + 1) % waypoints.Count;
                route = map.FindPath(location.coordinates, waypoints[wayPos]);
                routePos = 0;
            }
            MoveToRoom(map.GetRoom(route[routePos]));
            routePos += 1;
        }
    }

    public void DisplayIntent(){
        switch(state){
            case Behavior.patrol:
            if (route.Count == 0){
                route = map.FindPath(location.coordinates, waypoints[routePos]);
            }
                moveIntent.Points = new Vector2[2] {Vector2.zero, (route[routePos]- location.coordinates) * 100};
                moveIntent.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void Update()
    {
        if (moving && Vector2.Distance(rt.anchoredPosition, location.rt.anchoredPosition) > 0.1){
            rt.anchoredPosition = Vector2.SmoothDamp(rt.anchoredPosition, location.rt.anchoredPosition, ref velocity, moveTime);
        } else if (moving){
            location.OnRoomEnter();
            moving = false;
            map.CheckForCombat(location, this);
            //DisplayIntent();
        }
    }
}
