using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Environment/Alarm Buffs")]
public class AlarmBuffs : EnvironmentBuffs
{
    
    public int allySupriseMod;
    public int enemySupriseMod;
    public float lootMod;

    public List<Character> enemySpawn;

    public List<Character> SpawnPatrol(){
        return enemySpawn;
    }

    public override string ToString(){
        string result = "";
        if (allySupriseMod > 0){
            result += string.Format("Chance to Surpise Enemies: +{0}%\n", allySupriseMod);
        }
        if (enemySupriseMod > 0){
            result += string.Format("Chance to be Suprised: +{0}%\n", enemySupriseMod);
        }
        result += string.Format("Loot Value: x{0}\n", lootMod);
        result += base.ToString();
        if (enemySpawn.Count > 0){
            result += "Spawn Enemy Patrol\n";
        }
        return result;
    }
}
