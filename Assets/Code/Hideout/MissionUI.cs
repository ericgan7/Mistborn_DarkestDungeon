using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MissionUI : MonoBehaviour
{
    public List<Mission> availableMissions;
    public List<HeroRosterSlot> characters;
    public MissionSlot slotPrefab;
    public MissionIcon iconPrefab;
    public GameObject missionParent;
    public GameObject mapParent;

    MissionSlot selectedMission;

    void Awake(){
        //TODO: Create icons on the map
        foreach (Mission m in availableMissions){
            MissionIcon mi = Instantiate<MissionIcon>(iconPrefab, mapParent.transform);
            mi.Init(m);
            MissionSlot mb = Instantiate<MissionSlot>(slotPrefab, missionParent.transform);
            mb.manager = this;
            mb.icon = mi;
            mb.SetMission(m);
        }
        selectedMission = null;
    }

    public void SelectMission(MissionSlot m){
        //Unhighlihght region;
        if (selectedMission != null){
            selectedMission.icon.SetTooltip(false);
        }
        selectedMission = m;
        selectedMission.icon.SetTooltip(true);
        //TODO: ENEMIES PREDICTION (XCOM)
        //TODO: HIGHLIGHT Map regions 
    }

    public List<Character> GetCharacters(){
        List<Character> selectedTeam = new List<Character>();
        foreach (HeroRosterSlot cs in characters){
            selectedTeam.Add(cs.GetCharacter());
        }
        return selectedTeam;
    }

    public void StartMission(){
        //CHECK SELECTED CHARACTERS
        if (selectedMission == null){
            return;
        }
        List <Character> team = GetCharacters();
        if (team.Count == 0){
            return;
        }
        GameState gs = FindObjectOfType<GameState>();
        gs.hr.SetCurrentCharacters(team);
        //placeholder
        Debug.Log("starting_mission");
        SceneManager.LoadSceneAsync("1_IntroScene");
    }
}
