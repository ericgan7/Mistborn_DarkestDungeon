using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Create Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public Sprite weaponIcon;
    //TODO WeaponStats; damage, speed, acc, stealth
    public Vector2Int damage;
    public List<Ability> weaponAbilities;
    public Ability steelSpecial;
    public Ability ironSpecial;
    public Ability tinSpecial;
    public Ability petwerSpecial;
    public Ability zincSpecial;
    public Ability brassSpecial;

    public string description;

    public List<Ability> GetAbilities(string characterClass)
    {//Potential TODO: make class enum?
        List<Ability> results = new List<Ability>(weaponAbilities);
        switch (characterClass)
        {
            case "Steel":
                results.Add(steelSpecial);
                break;
            case "Iron":
                results.Add(ironSpecial);
                break;
            case "Tin":
                results.Add(tinSpecial);
                break;
            case "Pewter":
                results.Add(petwerSpecial);
                break;
            case "Zinc":
                results.Add(zincSpecial);
                break;
            case "Brass":
                results.Add(brassSpecial);
                break;
        }
        return results;
    }
}
