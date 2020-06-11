using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Environment Buffs")]
public class EnvironmentBuffs : ScriptableObject
{
    public int stress;
    public List<string> allyEffects;
    public List<string> enemyEffects;
    public int metalMod;

    public int level;

    public List<StatusEffect> GetEffects(Team team){
        List<StatusEffect> result = new List<StatusEffect>();
        foreach (string effect in team.isAlly ? allyEffects : enemyEffects){
            result.Add(EffectLibrary.GetEffect(effect) as StatusEffect);
        }
        return result;
    }

    public int StressOnTurnStart(){
        return stress;
    }

    public virtual string GetString(){
        string result = "";
        if (metalMod != 0){
            result += string.Format("Metallic Ability Cost {0}{1}\n", metalMod > 0 ? "+" : "-", metalMod);
        }
        if (allyEffects.Count > 0) result += "Allies:\n";
        foreach (string s in allyEffects){
            result += EffectLibrary.GetEffect(s).ToString() + "\n";
        }
        if (enemyEffects.Count > 0) result += "Enemies:\n";
        foreach (string s in enemyEffects){
            result += EffectLibrary.GetEffect(s).ToString() + "\n";
        }
        return result;
    }
}
