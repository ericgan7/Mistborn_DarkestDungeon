using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceManager
{
    public int gold;
    public int intel;
    public int metal;
    public int weaponParts;

    public void load(){
        gold = 5000;
        intel = 100;
        metal = 5;
        weaponParts = 0;
    }
}
