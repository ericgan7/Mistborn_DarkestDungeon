using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class AlarmManager : MonoBehaviour
{
    //alarm;
    public Alarm alarm;
    int alarmLevel;
    public List<AlarmBuffs> alarmBuffs;

    public void Awake()
    {
        GameState.Instance.am = this;
        alarmLevel = alarm.alarmLevel;
    }

    public EnvironmentBuffs GetAlarmEffects(){
        return alarmBuffs[alarmLevel];
    }

    public void OnAlarmIncrease(int level){
        alarmLevel = level;
        foreach (Unit u in GameState.Instance.ally.GetUnits()){
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

    public void SetAlarmVisible(bool isOn){
        alarm.gameObject.SetActive(isOn);
    }
}
