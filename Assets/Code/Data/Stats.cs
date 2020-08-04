using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    Character character;
    Unit unit;
    public StatusEffectManager modifiers;

    Vector2Int health;
    Vector2Int defense;
    Vector2Int will;
    Vector2Int damage;

    public Stats(Character c, Unit u, StatusEffectManager manager)
    {
        character = c;
        unit = u;
        modifiers = manager;
        health = new Vector2Int(c.health, c.health);
        defense = new Vector2Int(c.defense, c.defense);
        will = new Vector2Int(0, c.will);
        if (c.currentWeapon == null){
            damage = new Vector2Int(c.dmg, c.dmg); //TODO placeholder
        } else 
        damage = new Vector2Int(c.dmg + c.currentWeapon.damage.x, c.dmg + c.currentWeapon.damage.y);
    }

    public void Init(){
        health.y += (int) modifiers.GetStatModifier(StatType.health);
        health.x = health.y;
        defense.y+= (int) modifiers.GetStatModifier(StatType.defense);
        defense.x = defense.y;
    }

    public string GetName() { return character.characterName; }
    public string GetClassName(){ return character.characterClass ? character.characterClass.className : character.characterName; }
    public string GetWeaponName() {return character.currentWeapon.weaponName; }
    public string GetSpriteHeader() {return character.GetSpriteHeader(); }
    public Vector2Int Health() { return health; }
    public Vector2Int Defense() { return defense; }
    public Vector2Int Will() { return will; }
    public Vector2Int Damage() { 
        Vector2Int d = damage;
        int modifier = (int) modifiers.GetStatModifier(StatType.damage);
        d.x += modifier;
        d.y += modifier;
        return damage; 
    }
    public Weapon GetWeapon() { return character.currentWeapon; }

    public int GetStat(StatType type, Unit target = null)
    {
        switch (type)
        {
            case StatType.damage:
                return (int) (Random.Range(damage.x, damage.y)
                    + modifiers.GetStatModifier(type, target));
            case StatType.acc:
                return (int)(character.acc + modifiers.GetStatModifier(type));
            case StatType.crit:
                return (int)(character.crit + modifiers.GetStatModifier(type));
            case StatType.dodge:
                return (int)(character.dodge + modifiers.GetStatModifier(type));
            case StatType.speed:
                return (int)(character.speed + modifiers.GetStatModifier(type));
            case StatType.bleedResist:
                return (int)(character.bleedResist + modifiers.GetStatModifier(type));
            case StatType.moveResist:
                return (int)(character.bleedResist + modifiers.GetStatModifier(type));
            case StatType.debuffResist:
                return (int)(character.debuffResist + modifiers.GetStatModifier(type));
            case StatType.stunResist:
                return (int)(character.stunResist + modifiers.GetStatModifier(type));
            case StatType.stressResist:
                return (int)(character.stressResist + modifiers.GetStatModifier(type));
        }
        return 0;
    }

    public List<Ability> GetAbilities() {
        List<Ability> abilities = new List<Ability>(character.abilities); 
        if (character.movement != null){
            abilities.Add(character.movement);
        }
        return abilities;
    }
    public List<EquipableItem> GetItems(){
        return character.equippedItems;
    }
    public List<Ability> GetItemAbilities() {
        List<Ability> itemAbilities = new List<Ability>();
        Ability ability;
        foreach(EquipableItem i in character.equippedItems){
            if (i == null){
                itemAbilities.Add(null);
                continue;
            }
            ability = i.GetAbility();
            itemAbilities.Add(ability);
        }
        return itemAbilities;
    }

    public void TakeDamage(int amount)
    {
        //armor takes damage
        if (defense.x > amount)
        {
            defense.x -= amount;
            amount = 0;
        }
        else if (defense.x > 0)
        {
            amount -= defense.x;
            defense.x = 0;
        }
        //health takes damage
        health.x = Mathf.Clamp(health.x - amount, 0, health.y);
        if (health.x <= 0){
            Debug.Log(health.x);
            unit.Die(); 
        }
    }
    public void Heal(int amount)
    {
        health.x = Mathf.Clamp(health.x + amount, 0, health.y);
    }
    public void GainArmor(int amount)
    {
        defense.x = Mathf.Clamp(defense.x + amount, 0, defense.y);
    }

    public bool Block()
    {
        if (modifiers.IsBlock)
        {
            modifiers.BlockAttack();
            return true;
        }
        return false;
    }

    public void StressDamage(int amount){
        will.x = Mathf.Clamp(will.x + amount, 0, will.y);
        if (will.x == will.y){
            if (unit.UnitTeam.isAlly && modifiers.Affliction == null){
                will.x = 0;
                SetAffliction();
            } else {
                unit.Die();
            }
        }
    }

    public void ApplyDelayedEffect(StatusEffect e)
    {
        modifiers.AddEffect(e);
        //animations - buff applyed text, update status bar.
    }

    public bool OnTurnStart(){
        if (health.x <= 0){
            return false;
        }
        bool startTurn = true;
        if (modifiers.IsStunned){
            startTurn = false;
            modifiers.ClearStun();
        }
        int damage = 0;
        foreach (StatusEffect e in modifiers.Bleed){
            damage += e.ApplyOverTime(unit);
        }
        if (damage > 0){
            unit.CreatePopUpText(damage.ToString(), ColorPallete.GetColor("Red"));
        }
        int stress = 0;
        foreach (StatusEffect e in modifiers.Terror){
            stress += e.ApplyOverTime(unit);
        }
        if (stress > 0){
            unit.CreatePopUpText(stress.ToString(), ColorPallete.GetColor("Purple"));
        }
        modifiers.OnTurnBegin();
        if (health.x <= 0){
            return false;
        }
        return startTurn;
    }

    public List<Traits> GetTraits(){
        List<Traits> traits = new List<Traits>(character.traits);
        return traits;
    }

    public List<TraitTarget> GetTraitTargets(){
        List<TraitTarget> traits = new List<TraitTarget>(character.characterClass.attackBonuses);
        return traits;
    }

    public void SetAffliction(){
        //may need to add modifiers to virtue, or make it a stat from will/stress resist;
        Affliction affliction = EffectLibrary.GenerateAffliction(character.virtue);
        modifiers.Affliction = affliction;
        unit.Panic();
    }

}
