using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    GameState state;
    Unit currentCharacter;
    public Unit CurrentCharacter {get{return currentCharacter;}}
    public AbilityButton[] abilityButtons;
    public Image portrait;
    [SerializeField] TextMeshProUGUI characterName;
    public TextMeshProUGUI[] stats;

    public List<Unit> unitTargets;

    public Slider currentTurn;

    public ActionDisplay action;
    public GameObject background;

    public AlarmManager alarm;

    public GraphicRaycaster uiRayCaster;

    public AbilityMenu abilityMenu;
    public EquipmentMenu equipmentMenu;
    public ToggleMenu ability_map;

    public ItemTray items;

    [SerializeField] DetailedCharacterMenu characterMenu;
    [SerializeField] GameObject menu;

    private void Awake()
    {
        state = FindObjectOfType<GameState>();
        state.uic = this;
    }

    public void SetCurrentUnit(Unit current)
    {
        currentCharacter = current;
        foreach(Unit u in state.ally.GetUnits()){
            u.SetTargetState(TargetedState.Untargeted);
        }
        currentCharacter.SetTargetState(TargetedState.Current);
        abilityMenu.SetUnit(current);
        //TODO get protrait sprite
        characterName.text = current.stats.GetName();
        equipmentMenu.SetUnit(current);
        for (int i = 0; i < stats.Length; ++i)
        {
            stats[i].text = GetStatText(current, i);
        }
        items.SetIconUsable(current, state.gc.mode);
    }

    public void SelectAbility(Ability a)
    {
        DeselectAbility();
        a.SetTargets(currentCharacter, state.ally, state.enemy);
    }
   
    public void DeselectAbility()
    {
        for (int i = 0; i < unitTargets.Count; ++i)
        {
            unitTargets[i].SetTargetState(TargetedState.Untargeted);
            unitTargets[i].SetToolTip(null, null);
        }
        currentCharacter.SetTargetState(TargetedState.Current);
    }

    public void ResetTargetState()
    {
        foreach (Unit u in unitTargets)
        {
            u.SetTargetState(TargetedState.Untargeted);
        }
    }

    public string GetStatText(Unit c, int index)
    {
        switch (index)
        {
            case 0:
                return c.stats.Health().ToString();
            case 1:
                return c.stats.Defense().ToString();
            case 2:
                return c.stats.Will().ToString();
            case 3:
                return c.stats.GetStat(StatType.acc).ToString();
            case 4:
                return c.stats.Damage().ToString();
            case 5:
                return c.stats.GetStat(StatType.crit).ToString();
            case 6:
                return c.stats.GetStat(StatType.dodge).ToString();
            case 7:
                return c.stats.GetStat(StatType.speed).ToString();
            default:
                return "";
        }
    }

    public void ScaleBackground(float scale, float time){
        StartCoroutine(ChangeZoom(background, Vector3.one * scale, time));
    }

    IEnumerator ChangeZoom(GameObject target, Vector3 scale, float time){
        Vector3 velocity = Vector3.zero;
        float elapsed = 0f;
        while (elapsed < time){
            target.transform.localScale = Vector3.SmoothDamp(
                target.transform.localScale,
                scale,
                ref velocity,
                time
            );
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

    }

    public void ToggleMenu(string menu){
        switch(menu){
            case "Ability":
            case "Map":
                ability_map.SelectToggle(menu);
                break;
        }
    }

    public void SetCharacterMenu(Unit focus, Character character){
        characterMenu.SetUnit(focus, character);
        characterMenu.gameObject.SetActive(true);
    }   

    public void OpenGameOverMenu(){
        menu.SetActive(true);
    }

}
