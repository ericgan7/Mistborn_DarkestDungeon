using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Dialogue/Manager")]
public class EventLibrary : ScriptableObject
{
    public List<Event> story;
    public List<Event> events;

    public Dictionary<int, Event> storyLibrary;
    public Dictionary<int, Event> eventLibrary;

    public void Init(){
        storyLibrary = new Dictionary<int, Event>();
        eventLibrary = new Dictionary<int, Event>();

        foreach(Event d in story){
            if (storyLibrary.ContainsKey(d.dialogueId)){
                Debug.LogError("REPEATED DIALOGUE ID");
            }
            storyLibrary.Add(d.dialogueId, d);
        }
        //TODO: events
    }

    public Event GetDialogue(int id){
        if (!storyLibrary.ContainsKey(id)){
            Debug.LogError("DIALOGUE NOT FOUND");
        }
        return storyLibrary[id];
    }
}
