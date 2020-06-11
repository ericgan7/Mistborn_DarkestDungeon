using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Dialogue/Choice")]
public class Choice : ScriptableObject
{
    public int choiceId;
    public string text;
    public bool endOfDialogue;
    public bool startCombat;
    public bool startMap;
    [SerializeField] List<Reward> reward;

    [SerializeField] List<Reward> failure;
    [SerializeField] Event nextDialogue;

    [SerializeField] StatType statRoll;
    [SerializeField] int requiredRoll;
    [SerializeField] UsableItem requiredItem;

    public List<Reward> Reward { get {return reward;}}
    public List<Reward> Failure {get{return failure;}}
    public Event Dialogue {get {return nextDialogue;}}
    public bool IsEnd() { return endOfDialogue || startCombat; }

    public bool ValidOption(Inventory inventory){
        if (requiredItem != null) {
            if (inventory.Contains(requiredItem)){
                return true;
            }
            return false;
        } else {
            return true;
        }
    }
    public bool RollSuccess(Unit unit, Inventory inventory){
        if (requiredItem != null){
            inventory.UseItem(requiredItem);
            return true;
        }
        if (statRoll == StatType.none) {
            return true;
        }
        else {
            int stat = unit.stats.GetStat(statRoll);
            if (Random.Range(0, stat) > requiredRoll){
                return true;
            }
            return false;
        }
    }
}
