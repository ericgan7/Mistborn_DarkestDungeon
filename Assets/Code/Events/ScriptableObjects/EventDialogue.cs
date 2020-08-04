using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Event/New Event")]
public class EventDialogue : ScriptableObject
{
    public List<string> dialogueText;
    public List<EventChoice> choices;
    public Sprite eventImage; 
    //consider adding [tags] to determine the speaker
    public Character eventStats;
    public EventOutcome ignore;
}
