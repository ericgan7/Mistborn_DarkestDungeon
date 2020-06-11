using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Mission/Map")]
[System.Serializable]
public class MapLayout : ScriptableObject
{
    public List<Vector2Int> rooms;
    //available traps/curio rooms and types

    public List<Vector2Int> patrolPoints;

    public MapLayout(){
        rooms = new List<Vector2Int>();
    }
}
