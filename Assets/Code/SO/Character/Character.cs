﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Create Character")]
public class Character : ScriptableObject
{
    // Character variables
    public string characterName;
    public string className;
    public Sprite portrait;

    public List<Ability> abilities;
    public Ability movement;
    public List<EquipableItem> equippedItems;

    public List<string> ctraits;
    public List<Traits> traits;

    public CharacterClass characterClass;
    public Weapon currentWeapon;

    public int health;
    public int defense;
    public int will;

    public int crit;
    public int acc;
    public int dmg;
    public int speed;
    public int dodge;

    public int bleedResist;
    public int stunResist;
    public int moveResist;
    public int debuffResist;
    public int stressResist;

    public EnemyAI ai;

    public Character(Character c)
    {
        characterName = c.characterName;
        className = c.className;
        abilities = new List<Ability>(c.abilities);
        movement = c.movement;
        equippedItems = new List<EquipableItem>(c.equippedItems);
        traits = new List<Traits>();
        foreach(string s in c.ctraits){
            traits.Add(EffectLibrary.GetTraits(s));
        }
        characterClass = c.characterClass;
        currentWeapon = c.currentWeapon;

        health = c.health;
        defense = c.defense;
        will = c.will;
        crit = c.crit;
        acc = c.acc;
        dmg = c.dmg;
        speed = c.speed;
        dodge = c.dodge;

        bleedResist = c.bleedResist;
        stunResist = c.stunResist;
        moveResist = c.moveResist;
        debuffResist = c.moveResist;

        ai = c.ai;
    }

    public string GetBasicSprite()
    {
        return string.Format("{0} {1}", characterClass.className, currentWeapon.weaponName); 
    }

    public string GetSpriteSkill(Ability a)
    {
        return string.Format("{0} {1} {2}", characterClass.className, currentWeapon.weaponName, a.abilityName); 
    }
}
