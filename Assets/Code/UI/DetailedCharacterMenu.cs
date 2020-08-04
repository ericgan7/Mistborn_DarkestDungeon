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
    public List<WeaponSelection> weapons;
    public List<TextMeshProUGUI> stats;
    public List<TraitsTooltip> traits;
    public List<ItemSlot> slots;

    [SerializeField] TraitDescription classTrait;
    [SerializeField] TraitDescription traitPrefab;
    [SerializeField] GameObject traitParent;
    List<TraitDescription> traitDescriptions;

    public void Awake(){
        state = FindObjectOfType<GameState>();
        Init();
    }

    public void Init(){
        if (traitDescriptions == null){
            traitDescriptions = new List<TraitDescription>();
        }
    }

    public bool CanChangeAbilities(){
        return state.gc == null || state.gc.mode != GameMode.combat;
    }

    public void Activate(bool isOn){
        gameObject.SetActive(isOn);
        if (isOn){
            gameObject.transform.SetAsLastSibling();
        }
    }

    //TODO add character portrait
    public void SetUnit(Unit unit, Character character){
        currentCharacter = character;
        unitPortrait.sprite = SpriteLibrary.GetCharSprite(character.GetSpriteHeader() + "_Default");
        portrait.sprite = SpriteLibrary.GetPortrait(character.characterClass.className);
        characterName.text = character.characterName;
        SetAbilities();
        SetStats(unit);
        SetWeapons();
        SetTraits();
    }

    public void SetCharacter(Character character){
        currentCharacter = character;
        characterName.text = character.name;
        unitPortrait.sprite = SpriteLibrary.GetCharSprite(character.GetSpriteHeader() + "_Default");
        portrait.sprite = SpriteLibrary.GetPortrait(character.characterClass.className);
        SetAbilities();
        SetStats();
        SetWeapons();
        SetTraits();
    }

    void SetAbilities(){
        int i = 0;
        foreach(Ability a in currentCharacter.characterClass.classAbilities){
            abilities[i].SetAbility(a, currentCharacter.abilities.Contains(a));
            ++i;
        }
        foreach(Ability a in currentCharacter.currentWeapon.GetAbilities(currentCharacter.characterClass.className)){
            abilities[i].SetAbility(a, currentCharacter.abilities.Contains(a));
            ++i;
        }
        foreach(EquipableItem ei in currentCharacter.equippedItems){
            if (ei == null){
                abilities[i].SetAbility(null, true);
                ++i;
                continue;
            }
            abilities[i].SetAbility(ei.GetAbility(), true);
            ++i;
        }
    }

    public void SetWeapons(){
        int i = 0;
        foreach(Weapon w in currentCharacter.weapons){
            weapons[i].SetWeaponImage(w);
            if (w == currentCharacter.currentWeapon){
                weapons[i].SelectWeapon();
            }
            ++i;
        }
    }

    public void SetTraits(){
        if (traitDescriptions == null){
            Init();
        }
        foreach(TraitDescription t in traitDescriptions){
            Destroy(t.gameObject);
        }
        classTrait.SetTrait(currentCharacter.traits[0]);
        for (int i = 1; i < currentCharacter.traits.Count; ++i){
            TraitDescription t = Instantiate<TraitDescription>(traitPrefab, traitParent.transform);
            t.SetTrait(currentCharacter.traits[i]);
            traitDescriptions.Add(t);
        }
    }

    void SetStats(){
        for (int i = 0; i < stats.Count; ++i){
            switch (i){
                case 0:
                    stats[i].text = currentCharacter.defense.ToString();
                    break;
                case 1:
                    stats[i].text = currentCharacter.health.ToString();
                    break;
                case 2:
                    stats[i].text = currentCharacter.will.ToString();
                    break;
                case 3:
                    stats[i].text = currentCharacter.acc.ToString();
                    break;
                case 4:
                    stats[i].text = string.Format("{0} - {1}", 
                        (currentCharacter.dmg + currentCharacter.currentWeapon.damage.x).ToString(),
                        (currentCharacter.dmg + currentCharacter.currentWeapon.damage.y).ToString());
                    break;
                case 5:
                    stats[i].text = string.Format("{0}%", currentCharacter.crit.ToString());
                    break;
                case 6:
                    stats[i].text = currentCharacter.speed.ToString();
                    break;
                case 7:
                    stats[i].text = currentCharacter.dodge.ToString();
                    break;
                case 8:
                    stats[i].text = currentCharacter.bleedResist.ToString();
                    break;
                case 9:
                    stats[i].text = currentCharacter.stunResist.ToString();
                    break;
                case 10:
                    stats[i].text = currentCharacter.moveResist.ToString();
                    break;
                case 11:
                    stats[i].text = currentCharacter.debuffResist.ToString();
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

    public void SelectWeapon(Weapon weapon){
        if (weapon == currentCharacter.currentWeapon){
            return;
        }
        foreach(WeaponSelection w in weapons){
            if (w.isWeapon(currentCharacter.currentWeapon)){
                w.DeselectWeapon();
            }
        }
        foreach(Ability a in currentCharacter.currentWeapon.GetAbilities(currentCharacter.characterClass.className)){
            if (currentCharacter.abilities.Contains(a)){
                currentCharacter.abilities.Remove(a);
            }
        }
        currentCharacter.currentWeapon = weapon;
        int i = 4;
        foreach (Ability a in weapon.GetAbilities(currentCharacter.characterClass.className)){
            abilities[i].SetAbility(a, false);
            ++i;
        }
        unitPortrait.sprite = SpriteLibrary.GetCharSprite(currentCharacter.GetSpriteHeader() + "_Default");
    }
}
