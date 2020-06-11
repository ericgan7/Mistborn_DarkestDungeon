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

    public List<Character> GetAllyCharacters()
    {
        //can either generate instance with create instance or use prefab;
        List<Character> allies = new List<Character>();
        for (int i = 0; i < 4; ++i)
        {
            if (i < selectedCharacters.Count)
            {
                allies.Add(new Character(selectedCharacters[i]));
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
        foreach(Character c in enemyPrefabs[0].characters){
            if (c != null){
                team.Add(new Character(c));
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
