using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TeamResource
{
    public int gold;
    public int intel;
    public int metal;
    public int supplies;
    public int components;

    public void load(){
        gold = 5000;
        intel = 100;
        metal = 5;
        supplies = 10;
        components = 10;
    }
}

public enum ResourceType{
    gold,
    intel,
    metal,
    supplies,
    components
}
