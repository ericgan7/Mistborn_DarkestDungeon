using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Create Character")]
public class Character : ScriptableObject
{
    // Character variables
    public string characterName;
    public string className;

    public List<Ability> abilities; //current abilities
    //TODO: way to keep track of skills learned;
    public CharacterClass characterClass;
    public Weapon currentWeapon;

    public Vector2Int health;
    public Vector2Int defense;
    public Vector2Int will;
    public int crit;
    public int acc;
    public Vector2Int dmg;
    public int speed;
    public int dodge;

    public Character(Character c)
    {
        abilities = new List<Ability>(c.abilities);
        characterClass = c.characterClass;
        currentWeapon = c.currentWeapon;
        health = new Vector2Int(c.health.x, c.health.y);
        defense = new Vector2Int(c.defense.x, c.defense.y);
        will = new Vector2Int(c.will.x, c.defense.y);
        crit = c.crit;
        acc = c.acc;
        dmg = new Vector2Int(c.dmg.x, c.dmg.y);
        speed = c.speed;
        dodge = c.dodge;

        foreach (Ability a in abilities)
        {
            a.Init();
        }
    }
}
