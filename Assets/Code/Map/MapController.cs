using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;
using UnityEngine.UI;

public class AstarNode{
    public AstarNode(Vector2Int pos, int pred, int pa){
        position = pos;
        prediction = pred;
        path = pa;
    }
    public Vector2Int position;
    public int prediction;
    public int path;
    public int Cost(){
        return prediction + path;
    }
}

public class AstarComparer : IComparer<AstarNode>{
    public int Compare(AstarNode a, AstarNode b){
        if (a.Cost() > b.Cost()){
            return 1;
        } else if (a.Cost() < b.Cost()){
            return -1;
        }
        else {
            if (a.position == b.position){
                return 0;
            }
            return a.position.x > b.position.x ? 1 : -1;
        }
    }
}

public class MapController : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform map;
    public UILineRenderer line;
    Vector3 startPosition;
    Vector3 mapPosition;
    HashSet<Vector2Int> foundConnections;
    Dictionary<Vector2Int, Room> rooms;
    GameState state;

    public MapUnit player;
    public GameObject units;
    public List<MapUnit> patrols;

    public Image background;
    public Image transition;
    public float transitionTime;
    public EventObject eventObject;
    bool canScroll;

    public bool loadLevel;
    public MapLayout level;
    public Room prefab;
    static List<Vector2Int> offsets = new List<Vector2Int> {
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
    };

    public List<Vector2Int> test;
    public void Awake()
    {
        rooms = new Dictionary<Vector2Int, Room>();
        foundConnections = new HashSet<Vector2Int>();
        state = FindObjectOfType<GameState>();
        state.map = this;
        patrols = new List<MapUnit>(units.GetComponentsInChildren<MapUnit>());
    }
    
    void Start(){
        if (loadLevel){
            LoadLevel(level);
        } else {
            foreach(Room r in FindObjectsOfType<Room>()){
            rooms.Add(r.coordinates, r);
            }
        }
        player.SetPosition(rooms[Vector2Int.zero]);
        UpdateMapState();
    }

    public void LoadLevel(MapLayout layout){
        foreach(Vector2Int v in level.rooms){
            Room r = Instantiate<Room>(prefab, map.transform);
            r.SetPostion(v);
            r.map = this;
            rooms.Add(v, r);
        }
        foreach(MapUnit u in patrols){
            //placeholder set up
            u.SetPosition(rooms[layout.patrolPoints[1]]);
            u.SetRoute(layout.patrolPoints);
        }
        units.transform.parent = gameObject.transform;
        units.transform.parent = map.transform;
    }
    
    public Room GetRoom(Vector2Int coord){
        return rooms[coord];
    }

    public void Open(){
        gameObject.SetActive(true);
    }

    public void StartEvent(Event dialogue){
        state.dm.Open(dialogue);
        gameObject.SetActive(false);
    }

    public void UpdateMapState(){
        foreach (Room r in rooms.Values){
            r.HideRoom();
        }
        BFS(player.location, player.vision, RoomState.revealed);
    }

    void BFS(Room room, int level, RoomState targetState){
        if (room.state == targetState){
            return;
        }
        room.SetRoom(targetState);
        if (level > 0){
            foreach(Room r in NeighboringRooms(room)){
                BFS(r, level -1, targetState);
            }
        }
    }

    List<Room> NeighboringRooms(Room room){
        List<Room> results = new List<Room>();
        foreach (Vector2Int offset in offsets){
            Vector2Int pos = Vector2Int.zero;
            pos.x = offset.x + room.coordinates.x;
            pos.y = offset.y + room.coordinates.y;
            if (rooms.ContainsKey(pos)){
                results.Add(rooms[pos]);
            }
        }
        return results;
    }

    //TODO: Check for multiple fights one after another?
    public void CheckForCombat(Room room, MapUnit agent){
        if (agent.isPlayer){
            foreach(MapUnit u in patrols){
                if (u.isPlayer){
                    continue;
                }
                //within enemy vision
                if (room.MDistance(u.location) <= u.vision){
                    u.MoveToRoom(agent.location);
                    state.gc.StartCombat(u.units);
                    EnemyEntrance();
                }
            }
        } else {
            if (room.MDistance(player.location) == agent.vision){
                agent.MoveToRoom(player.location);
                state.gc.StartCombat(agent.units);
                EnemyEntrance();
            }
        }
    }

    public void MoveToRoom(Room selected)
    {
        if (player.location.IsNear(selected))
        {
            StartCoroutine(BackgroundTransition(selected.backgroundImage));
            player.MoveToRoom(selected);
            state.am.IncreaseAlarm();
            UpdateMapState();
        }
        if (selected.enterEvent){
            eventObject.SetEvent(selected.enterEvent);
        }
        else {
            eventObject.gameObject.SetActive(false);
        }
    }

    public IEnumerator BackgroundTransition(Sprite newImage)
    {
        float elapsed = 0f;
        Color current = Color.black;
        while (elapsed < transitionTime){
            elapsed += Time.deltaTime;
            current.a = Mathf.Lerp(0, 1, elapsed / transitionTime); //fade in black
            transition.color = current;
            yield return new WaitForEndOfFrame();
        }
        background.sprite = newImage;
        foreach (Unit u in state.ally.GetUnits()){
            u.SetPosScale(state.ally.positions[2], Vector3.one);
            u.ResetPosition();
        }
        elapsed = 0f;
        while (elapsed < transitionTime){
            elapsed += Time.deltaTime;
            current.a = Mathf.Lerp(1, 0, elapsed / transitionTime); //fade out black
            transition.color = current;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.5f);
        //start enemy turn;
        EnemyTurn();
    }

    public void EnemyEntrance(){
        foreach (Unit u in state.enemy.GetUnits()){
            u.SetPosScale(state.ally.positions[2], Vector3.one);
            u.ResetPosition();
        }
    }

    public List<Vector2Int> FindPath(Vector2Int currentPosition, Vector2Int targetPostion){
        if (currentPosition == targetPostion){
            return new List<Vector2Int>(){currentPosition};
        }
        Dictionary<Vector2Int, Vector2Int> paths = new Dictionary<Vector2Int, Vector2Int>();
        SortedSet<AstarNode> frontier = new SortedSet<AstarNode>(new AstarComparer());
        paths.Add(targetPostion, targetPostion);
        frontier.Add(new AstarNode(targetPostion, 0,0));
        int iter = 0;
        while(frontier.Count > 0){
            if (iter > 50){
                break;
            }
            iter++;
            AstarNode current = frontier.Min;
            frontier.Remove(current);
            if (current.position == currentPosition){
                break;
            }
            foreach(Room r in NeighboringRooms(rooms[current.position])){
                if (paths.ContainsKey(r.coordinates)){
                    continue;
                }
                paths.Add(r.coordinates, current.position);
                Vector2Int pred = r.coordinates - currentPosition;
                frontier.Add(new AstarNode(r.coordinates, Mathf.Abs(pred.x) + Mathf.Abs(pred.y), current.path + 1));
            }
        }
        if (!paths.ContainsKey(currentPosition)){
            Debug.Log("Pathfinding Error");
            return null;
        }
        List<Vector2Int> results = new List<Vector2Int>();
        Vector2Int p = paths[currentPosition];
        while(p != targetPostion){
            results.Add(p);
            p = paths[p];
        }
        results.Add(p);
        return results;
    }

    public void OnDrag(PointerEventData p)
    {
        mapPosition = map.anchoredPosition;
        map.anchoredPosition += p.delta;
    }

    public void OnPointerEnter(PointerEventData pointer){
        canScroll = true;
    }

    public void OnPointerExit(PointerEventData pointer){
        canScroll = false;
    }

    public void EnemyTurn(){
        foreach(MapUnit u in patrols){
            if (u.isPlayer){
                continue;
            }
            u.AdvanceTurn();
        }
    }
    public void Update(){
        const float minScale = 0.1f;
        const float maxScale = 3;
        const float positive_rate = 0.2f;
        const float negative_rate = 0.05f;
        if (canScroll){
            float rate = map.localScale.x < 1 ? negative_rate : positive_rate;
            Vector3 scale = new Vector3(
                Mathf.Clamp(Input.mouseScrollDelta.y * rate + map.localScale.x, minScale, maxScale), 
                Mathf.Clamp(Input.mouseScrollDelta.y * rate + map.localScale.y, minScale, maxScale));
            map.localScale = scale;
            
        }
    }
}


