using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Mission/Map")]
[System.Serializable]
public class MapLayout : ScriptableObject
{
    public List<Vector2Int> rooms;
    public List<EventDialogue> events;

    public List<Vector2Int> patrolPoints;

    public MapLayout(){
        rooms = new List<Vector2Int>();
    }
}
