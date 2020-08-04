using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Event/Outcome")]
public class EventOutcome : ScriptableObject
{
    public string description;
    public Ability_Event effects;
    public List<Item> items;
    public bool isEnd;

    public bool IsEffect { get { return effects != null; }}
    public bool IsItem { get { return items.Count > 0; }}
}
