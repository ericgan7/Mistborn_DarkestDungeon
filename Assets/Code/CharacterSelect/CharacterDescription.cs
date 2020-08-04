using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterDescription : MonoBehaviour
{
    [SerializeField] RectTransform menu;
    [SerializeField] TextMeshProUGUI characterName;
    [SerializeField] Image characterPortrait;

    [SerializeField] TextMeshProUGUI className;
    [SerializeField] TextMeshProUGUI classDescription;
    [SerializeField] AbilitySelection[] classAbilities;
    [SerializeField] TraitDescription classTrait;

    [SerializeField] WeaponSelection[] weaponToggles;
    [SerializeField] TextMeshProUGUI weaponDescription;
    [SerializeField] AbilitySelection[] weaponAbilities;

    Character currentCharacter;
    List<ContentSizeFitter> contents;
    List<VerticalLayoutGroup> layouts;

    void UpdateLayout(){
        LayoutRebuilder.ForceRebuildLayoutImmediate(menu);
    }
    
    public void SetCharacter(Character character){
        currentCharacter = character;
        characterName.text = character.characterName;
        characterPortrait.sprite = SpriteLibrary.GetPortrait(character.characterClass.className);
    
        className.text = character.characterClass.className;
        classDescription.text = character.characterClass.description;
        for (int i = 0; i < classAbilities.Length; ++i){
            classAbilities[i].SetAbility(character.characterClass.classAbilities[i], false);
        }
        classTrait.SetTrait(EffectLibrary.GetTraits(character.characterClass.className));

        for (int i = 0; i < weaponToggles.Length; ++i){
            weaponToggles[i].SetWeaponText(character.weapons[i]);
        }
        SelectWeapon(character.weapons[0]);
        weaponDescription.text = character.weapons[0].description;

        UpdateLayout();
        //UpdateLayout();
    }

    public void SelectWeapon(Weapon weapon){
        foreach(WeaponSelection w in weaponToggles){
            if (w.isWeapon(weapon)){
                continue;
            }
            w.DeselectWeapon();
        }
        List<Ability> abilities = weapon.GetAbilities(currentCharacter.characterClass.className);
        for (int i = 0; i < abilities.Count; ++i){
            weaponAbilities[i].SetAbility(abilities[i], false);
        }
        weaponDescription.text = weapon.description;
        UpdateLayout();
        //UpdateLayout();
    }
}
