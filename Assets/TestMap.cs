using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMap : MonoBehaviour
{

    
    public Room prefab;
    public int num_gen;
    public List<Room> rooms;
    public Vector2 size;
    public Transform parent;
    
    
    public void Start()
    {
        for (int i = 0; i < num_gen; ++i)
        {
            Room r = Instantiate<Room>(prefab);
            r.transform.localPosition = Random.insideUnitSphere;
            r.transform.localScale = new Vector3((int)Random.Range(size.x, size.y), (int)Random.Range(size.x, size.y));
            r.transform.parent = parent;
            r.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
            r.id = i;
            rooms.Add(r);
           
        }
        //StartCoroutine(SnapToGrid());
    }
}

public class Prims
{
    public HashSet<Room> found;
    //distances represents the min distance edge, index in arraay is room.id
    public List<float> distances;
    //origin represents the origin of the minimimum edge, distination is the index;
    public List<Room> origin;
    const float max_float = 1000000f;
    public Prims(List<Room> rooms)
    {
        found = new HashSet<Room>();
        distances = new List<float>(rooms.Count);
        origin = new List<Room>(rooms.Count);
        Room start = rooms[0];
        found.Add(start);
        //calculating initial distances
        foreach(Room r in rooms)
        {
            distances.Add(Vector3.SqrMagnitude(start.transform.localPosition - r.transform.localPosition));
            origin.Add(start);
        }
        //Primm's algorithm in fully connected graph
        while (found.Count < rooms.Count)
        {
            int minIndex = 0;
            float minVal = max_float;
            //get minimum distance
            for (int i = 0; i < distances.Count; ++i)
            {
                if (distances[i] < minVal)
                {
                    minVal = distances[i];
                    minIndex = i;
                }
            }
            //add minimum distance node to graph, remove it from contention
            found.Add(rooms[minIndex]);
            distances[minIndex] = max_float;
            //recalculate minimum distances
            float dist;
            for (int i = 0; i < rooms.Count; ++i)
            {
                //if edge is to a a new vertex
                if (!found.Contains(rooms[i]))
                {
                    //if distance is less than existing vertex;
                    dist = Vector3.SqrMagnitude(rooms[minIndex].transform.localPosition - rooms[i].transform.localPosition);
                    if (dist < distances[i])
                    {
                        distances[i] = dist;
                        
                    }
                }
            }
        }
    }

}
