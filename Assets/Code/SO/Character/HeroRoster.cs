using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Create Roster")]
public class HeroRoster : ScriptableObject
{
    public List<EnemyGroup> enemyPrefabs;
    public List<Weapon> weaponPrefabs;
    public List<CharacterClass> classPrefabs;

    public List<Character> currentRoster;

    public List<Character> selectedCharacters;

    public List<Character> GetRosterCharacters(){
        List<Character> roster = new List<Character>();
        foreach(Character c in currentRoster){
            Character character = ScriptableObject.CreateInstance("Character") as Character;
            character.Init(c);
            roster.Add(character);
        }
        return roster;
    }

    public List<Character> GetAllyCharacters()
    {
        //can either generate instance with create instance or use prefab;
        List<Character> allies = new List<Character>();
        for (int i = 0; i < 4; ++i)
        {
            if (i < selectedCharacters.Count)
            {
                Character character = ScriptableObject.CreateInstance("Character") as Character;
                character.Init(selectedCharacters[i]);
                allies.Add(character);
            }
            else
            {
                allies.Add(null);
            }
        }
        return allies;
    }

    public List<Character> GenerateEnemy(int amount)
    {
        //TODO randomly choose enmy prefabs within range
        //need to return copies of the scriptable object so they are inited properly
        List<Character> team = new List<Character>();
        foreach(Character c in enemyPrefabs[1].characters){
            if (c != null){
                Character character = ScriptableObject.CreateInstance("Character") as Character;
                character.Init(c);
                team.Add(character);
            }
            else {
                team.Add(null);
            }
        }
        return team;
    }

    public void LoadCharacters()
    {
        //TODO load from JSON file.
    }

    public void AddCharacter(Character c){
        selectedCharacters.Add(c);
        currentRoster.Add(c);
    }

    public void SetCurrentCharacters(List<Character> characters){
        selectedCharacters = characters;
    }
}
