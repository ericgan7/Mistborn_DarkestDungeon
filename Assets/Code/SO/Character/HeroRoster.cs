using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Create Roster")]
public class HeroRoster : ScriptableObject
{
    public List<Character> characterPrefabs;
    public List<Weapon> weaponPrefabs;
    public List<CharacterClass> classPrefabs;

    public List<Character> currentRoster;

    public Character GetCharacter(string heroClass, string weapon)
    {//can either generate instance with create instance or use prefab;
        Character c =  new Character(characterPrefabs[0]);
        return c;
    }

    public void LoadCharacters()
    {
        //TODO load from JSON file.
    }
}
