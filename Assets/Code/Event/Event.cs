using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Dialogue/Event")]
public class Event : ScriptableObject
{
    public int dialogueId;
    public string text;
    public Sprite dialogueImage;
    public Sprite dialogueObject;
    public List<Choice> choices;
}
