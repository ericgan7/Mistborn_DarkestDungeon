using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class AlarmManager : MonoBehaviour
{
    public int metalLevel;
    public int max_metal;
    public ProgressBar metalBar;
    //alarm;
    public Alarm alarm;
    int alarmLevel;
    public List<EnvironmentBuffs> metalBuffs;
    public List<AlarmBuffs> alarmBuffs;
    GameState state;

    public void Awake()
    {
        state = FindObjectOfType<GameState>();
        state.am = this;
        alarmLevel = alarm.alarmLevel;
        metalBar.Init(metalLevel, max_metal);
    }

    public void Start()
    {
        UpdateMetal(0);
    }

    public void UpdateMetal(int change)
    {
        metalLevel = Mathf.Clamp(metalLevel + change, 0, max_metal);
        metalBar.SetAmount(metalLevel);
    }

    public EnvironmentBuffs GetMetalEffects(){
        foreach(EnvironmentBuffs buffs in metalBuffs){
            if (metalLevel >= buffs.level){
                return buffs;
            }
        }
        return null;
    }

    public EnvironmentBuffs GetAlarmEffects(){
        return alarmBuffs[alarmLevel];
    }

    public void OnAlarmIncrease(int level){
        alarmLevel = level;
        foreach (Unit u in state.ally.GetUnits()){
            u.stats.StressDamage(alarmBuffs[level].stress);
        }
        //spawn new patrol;
    }

    public float GetLootMod(){
        return alarmBuffs[alarmLevel].lootMod;
    }

    public void IncreaseAlarm()
    {
        alarm.SetAlarmLevel(1);
    }

}
