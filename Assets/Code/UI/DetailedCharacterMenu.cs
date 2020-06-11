using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DetailedCharacterMenu : MonoBehaviour
{
    Character currentCharacter;
    GameState state;

    public Image portrait;
    public Image unitPortrait;
    public TextMeshProUGUI characterName;
    public List<AbilitySelection> abilities;
    public List<TextMeshProUGUI> stats;
    public List<TraitsTooltip> traits;
    public TraitsTooltip prefab;
    public List<ItemSlot> slots;

    public void Awake(){
        state = FindObjectOfType<GameState>();
    }

    public bool CanChangeAbilities(){
        return state.gc == null || state.gc.mode != GameMode.combat;
    }

    public void SetUnit(Unit unit, Character character){
        currentCharacter = character;
        portrait.sprite = character.portrait;
        unitPortrait.sprite = SpriteLibrary.GetCharSprite(currentCharacter.className + "_Default");
        characterName.text = character.characterName;
        SetAbilities(character);
        SetStats(unit);
    }

    public void SetCharacter(Character character){
        currentCharacter = character;
        portrait.sprite = character.portrait;
        characterName.text = character.name;
        SetAbilities(character);
        SetStats(character);
    }

    void SetAbilities(Character character){
        int i = 0;
        foreach(Ability a in character.characterClass.classAbilities){
            abilities[i].SetAbility(a, character.abilities.Contains(a));
            ++i;
        }
        foreach(Ability a in character.currentWeapon.weaponAbilities){
            abilities[i].SetAbility(a, character.abilities.Contains(a));
            ++i;
        }
        foreach(EquipableItem ei in character.equippedItems){
            if (ei == null){
                abilities[i].SetAbility(null, true);
                ++i;
                continue;
            }
            abilities[i].SetAbility(ei.GetAbility(), true);
            ++i;
        }
        abilities[i].SetAbility(character.movement, true);
    }

    void SetStats(Character character){
        for (int i = 0; i < stats.Count; ++i){
            switch (i){
                case 0:
                    stats[i].text = character.defense.ToString();
                    break;
                case 1:
                    stats[i].text = character.health.ToString();
                    break;
                case 2:
                    stats[i].text = character.will.ToString();
                    break;
                case 3:
                    stats[i].text = character.acc.ToString();
                    break;
                case 4:
                    stats[i].text = string.Format("{0} - {1}", 
                        (character.dmg + character.currentWeapon.damage.x).ToString(),
                        (character.dmg + character.currentWeapon.damage.y).ToString());
                    break;
                case 5:
                    stats[i].text = string.Format("{0}%", character.crit.ToString());
                    break;
                case 6:
                    stats[i].text = character.speed.ToString();
                    break;
                case 7:
                    stats[i].text = character.dodge.ToString();
                    break;
                case 8:
                    stats[i].text = character.bleedResist.ToString();
                    break;
                case 9:
                    stats[i].text = character.stunResist.ToString();
                    break;
                case 10:
                    stats[i].text = character.moveResist.ToString();
                    break;
                case 11:
                    stats[i].text = character.debuffResist.ToString();
                    break;
            }
        }
    }

    public void SetStats(Unit unit){
        for (int i = 0; i < stats.Count; ++i){
            switch (i){
                case 0:
                    stats[i].text = unit.stats.Defense().ToString();
                    break;
                case 1:
                    stats[i].text = unit.stats.Health().ToString();
                    break;
                case 2:
                    stats[i].text = unit.stats.Will().ToString();
                    break;
                case 3:
                    stats[i].text = unit.stats.GetStat(StatType.acc).ToString();
                    break;
                case 4:
                    Vector2Int dmg = unit.stats.Damage();
                    stats[i].text = string.Format("{0} - {1}", 
                        dmg.x.ToString(),
                        dmg.y.ToString());
                    break;
                case 5:
                    stats[i].text = string.Format("{0}%", unit.stats.GetStat(StatType.crit).ToString());
                    break;
                case 6:
                    stats[i].text = unit.stats.GetStat(StatType.speed).ToString();
                    break;
                case 7:
                    stats[i].text = unit.stats.GetStat(StatType.dodge).ToString();
                    break;
                case 8:
                    stats[i].text = unit.stats.GetStat(StatType.bleedResist).ToString();
                    break;
                case 9:
                    stats[i].text = unit.stats.GetStat(StatType.stunResist).ToString();
                    break;
                case 10:
                    stats[i].text = unit.stats.GetStat(StatType.moveResist).ToString();
                    break;
                case 11:
                    stats[i].text = unit.stats.GetStat(StatType.debuffResist).ToString();
                    break;
            }
        }
    }

    public void DeselectSkill(Ability ability){
        if (state.gc != null && state.gc.mode == GameMode.combat){
            return;
        }
        if (currentCharacter.abilities.Contains(ability)){
            currentCharacter.abilities.Remove(ability);
        }    
    }

    public bool SelectSkill(Ability ability){
        if (state.gc != null && state.gc.mode == GameMode.combat){
            return false;
        }
        if (currentCharacter.abilities.Count < 4){
            currentCharacter.abilities.Add(ability);
            return true;
        }
        return false;
    }

    public void Close(){
        gameObject.SetActive(false);
    }
}
