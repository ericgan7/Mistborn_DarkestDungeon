using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    GameState state;
    Unit currentCharacter;
    public AbilityButton[] abilityButtons;
    public Button portrait;
    public TextMeshProUGUI[] stats;

    public List<Unit> unitTargets;

    public Slider currentTurn;

    public ActionDisplay action;

    public AlarmManager alarm;

    private void Awake()
    {
        state = gameObject.GetComponent<GameState>();
        state.uic = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < abilityButtons.Length; ++i)
        {
            abilityButtons[i].index = i;
        }
    }

    public void SetCurrentUnit(Unit current)
    {
        currentCharacter = current;
        currentCharacter.SetTargetState(TargetedState.Current);
        for (int i = 0; i < abilityButtons.Length; ++i)
        {
            abilityButtons[i].SetAbility(current.stats.GetAbilities(i), currentCharacter);
        }
        for (int i = 0; i < stats.Length; ++i)
        {
            stats[i].text = GetStatText(current, i);
        }
    }

    public void SelectAbility(Ability a) //TODO Set default ability on turn start;
    {
        DeselectAbility();
        a.SetTargets(currentCharacter, state.ally, state.enemy);
    }
   
    public void DeselectAbility()
    {
        for (int i = 0; i < unitTargets.Count; ++i)
        {
            unitTargets[i].SetTargetState(TargetedState.Untargeted);
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
                return c.stats.Acc().ToString();
            case 4:
                return c.stats.Damage().ToString();
            case 5:
                return c.stats.Crit().ToString();
            case 6:
                return c.stats.Dodge().ToString();
            case 7:
                return c.stats.Speed().ToString();
            default:
                return "";
        }
    }

}
