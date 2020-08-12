using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class Team : MonoBehaviour
{
    [SerializeField] List<Unit> units;
    [SerializeField] Light2D teamLight;
    public bool isAlly;
    public List<Vector3> positions;
    public int supriseChance;
    public Vector3 direction;
    GameState state;

    private void Awake()
    {
        direction = isAlly ? Vector3.one : new Vector3(-1, 1, 1);
        positions = new List<Vector3>(units.Count);
        for (int i = 0; i < units.Count; ++i)
        {
            units[i].Location = i;
            units[i].UnitTeam = this;
            positions.Add(units[i].transform.localPosition);
        }
        state = FindObjectOfType<GameState>();
        if (isAlly)
        {
            state.ally = this;
        }
        else
        {
            state.enemy = this;
        }
        if (GetUnits().Count > 0){
            teamLight.enabled = true;
        }
        else {
            teamLight.enabled = false;
        }
    }

    public void SetUnits(EnemyGroup enemyUnits)
    {
        List<Character> characters;
        if (isAlly){
            characters = state.hr.GetAllyCharacters();
        }else if (enemyUnits != null) {
            characters = new List<Character>();
            foreach(Character c in enemyUnits.characters){
                if (c != null){
                    Character character = ScriptableObject.CreateInstance("Character") as Character;
                    character.Init(c);
                    characters.Add(character);
                }
                else {
                    characters.Add(null);
                }
            }
        } else{
            characters = state.hr.GenerateEnemy(0);//0 is testCode;
        }
        for (int i = 0; i < units.Count; ++i)
        {
            if (characters[i] == null)
            {
                units[i].gameObject.SetActive(false);
            }
            else
            {
                units[i].gameObject.SetActive(true);
                units[i].SetCharacter(characters[i], isAlly);
            }
        }
    }

    public void SetActionPips(){
        foreach(Unit u in GetUnits()){
            u.SetActionPips(true);
        }
    }


    public List<Unit> GetUnits()
    {
        List<Unit> result = new List<Unit>();
        foreach(Unit u in units){
            if (u.Active)
            {
                result.Add(u);
            }
        }
        return result;
    }

    public void MoveUnit(Unit target, int amount)
    {
        int index = target.Location;
        int newIndex = Mathf.Clamp(index + amount, 0, GetUnits().Count - 1);
        units.Remove(target);
        units.Insert(newIndex, target);
        for (int i = 0; i < units.Count; ++i)
        {
            units[i].Location = i;
            units[i].MoveUnit(positions[i], Vector3.one);
        }
    }

    public void TurnLightingOn(bool isOn){
        teamLight.enabled = isOn;
    }

}
