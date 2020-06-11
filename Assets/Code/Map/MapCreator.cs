using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;

public class MapCreator : MonoBehaviour, IPointerClickHandler
{
    public RectTransform rectTransform;
    public Dictionary <Vector2Int, Room> coords;
    public string MapName;
    public Room prefab;

    public void Awake(){
        coords = new Dictionary<Vector2Int, Room>();
        coords[Vector2Int.zero] = prefab;
    }

    public void Start(){
        prefab.roomImage.raycastTarget = false;
    }

    public void OnPointerClick(PointerEventData pointer){
        if (pointer.button == PointerEventData.InputButton.Left){
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, pointer.position, Camera.main, out pos);
            Vector2Int c = ConvertToRoomCoord(pos);
            if (coords.ContainsKey(c)){
                if (c == Vector2Int.zero){
                    return;
                }
                Destroy(coords[c].gameObject);
                coords.Remove(c);
            }
            else {
                Room r = Instantiate<Room>(prefab, rectTransform.transform);
                r.roomImage.raycastTarget = false;
                r.rt.anchoredPosition = new Vector2(c.x * prefab.rt.sizeDelta.x, c.y * prefab.rt.sizeDelta.y);
                coords[c] = r;
            }
        }
    }

    Vector2Int ConvertToRoomCoord(Vector2 pos){
        int x = pos.x < 0 ? (int) (pos.x - prefab.rt.sizeDelta.x / 2) / 100 : (int) (pos.x + prefab.rt.sizeDelta.x / 2) / 100;
        int y = pos.y < 0? (int) (pos.y - prefab.rt.sizeDelta.x / 2) / 100 : (int) (pos.y + prefab.rt.sizeDelta.x / 2) / 100;
        return new Vector2Int(x, y);
    }

    public void SaveToAssets(){
        if (MapName == ""){
            Debug.Log("Must Instantiate Name");
            return;
        }
        MapLayout layout = new MapLayout();
        foreach(Vector2Int coord in coords.Keys){
            layout.rooms.Add(coord);
        }
        AssetDatabase.CreateAsset(layout, "Assets/Scriptable_Objects/Map/" + MapName + ".asset");
    }
}
