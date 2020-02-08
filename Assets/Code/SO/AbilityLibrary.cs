using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create AbilityTable")]
public class AbilityLibrary : ScriptableObject
{
    public List<Ability> abilities;
    public Dictionary<string, Ability> abilityMap;

    public void InitMap()
    {
        foreach (Ability a in abilities)
        {
            abilityMap[a.name] = a;
        }
    }
    
    
}
