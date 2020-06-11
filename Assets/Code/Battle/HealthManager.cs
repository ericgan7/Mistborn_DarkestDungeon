﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] ProgressBar healthBar;
    [SerializeField] ProgressBar stressBar;
    [SerializeField] PipBar defenseBar;
    [SerializeField] Unit unit;
    [SerializeField] ActionPip actions;


    public void InitBars(){
        Vector2Int hp = unit.stats.Health();
        healthBar.Init(hp.x, hp.y);
        Vector2Int stress = unit.stats.Will();
        stressBar.Init(stress.x, stress.y);
        Vector2Int def = unit.stats.Defense();
        defenseBar.Init(def.x, def.y);
    }

    public void SetDamageIndicators(Vector2Int damageRange)
    {
        if (unit.stats.Defense().x > 0){
            defenseBar.SetIndicator(damageRange.x);
            int amount = Mathf.Min(damageRange.x, unit.stats.Defense().x);
            damageRange.x -= amount;      
        }
        if (damageRange.x >= 0){
            healthBar.SetIndicator(-1* damageRange.x);
        }
    }

    public void SetHealIndicators(Vector2Int healRange){
        
    }

    public void ExecuteUpdate(){
        healthBar.SetAmount(unit.stats.Health().x);
        stressBar.SetAmount(unit.stats.Will().x);
        defenseBar.SetAmount(unit.stats.Defense().x);
    }

    public void ShowIndicators(bool isOn){
        healthBar.ShowIndicator(isOn);
        defenseBar.ShowIndicator(isOn);
    }

    public void AddActionIndicator(){
        //TODO add functionality for multiple action pips
        //currently only allows one pip;
        actions.SetState(true);
    }

    public void RemoveActionIndicator(){
        actions.SetState(false);
    }

}
