using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    Character character;
    Unit unit;
    public StatusEffect counter;
    public List<StatusEffect> modifiers;

    public Stats(Character c, Unit u)
    {
        character = c;
        unit = u;
        modifiers = new List<StatusEffect>();
    }

    public string GetName() { return character.characterName; }
    public Vector2Int Health() { return character.health; }
    public Vector2Int Defense() { return character.defense; }
    public Vector2Int Will() { return character.will; }
    public int Damage() { return Random.Range(character.dmg.x, character.dmg.y); }
    public int Crit() { return character.crit; }
    public int Acc() { return character.acc; }
    public int Speed() { return character.speed; }
    public int Dodge() { return character.dodge; }
    public Ability GetAbilities(int i) { return character.abilities[i]; }

    public void TakeDamage(int amount)
    {
        //armor takes damage
        if (character.defense.x > amount)
        {
            character.defense.x -= amount;
            amount = 0;
        }
        else if (character.defense.x > 0)
        {
            amount -= character.defense.x;
            character.defense.x = 0;
        }
        //health takes damage
        character.health.x = Mathf.Clamp(character.health.x - amount, 0, character.health.y);
        //check if dead
        //update flag
    }
    public void Heal(int amount)
    {
        character.health.x = Mathf.Clamp(character.health.x + amount, 0, character.health.y);
    }
    public void GainArmor(int amount)
    {
        character.defense.x = Mathf.Clamp(character.defense.x + amount, 0, character.defense.y);
    }

    public void ApplyDelayedEffect(StatusEffect e)
    {
        modifiers.Add(e);
        //animations - buff applyed text, update status bar.
    }

    public void RemoveEffect(StatusEffect e)
    {
        modifiers.Remove(e);
        //TODO update UI to reflect removed effect.
        unit.UpdateUI();
    }

    public void CheckCounter(Ability a, Unit attacker, ref AbilityResultList results) {
        if (counter != null && a.IsAttack)
        {
            counter.Counterattack(attacker, this.unit, ref results);
        }

    }
}
