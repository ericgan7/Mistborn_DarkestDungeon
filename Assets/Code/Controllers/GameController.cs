using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Variables
    public GameState state;

    public List<Unit> turnorder;
    static CompareUnitSpeed compare = new CompareUnitSpeed();
    public HeroRoster hr;
    Unit currentUnit;


    private void Awake()
    {
        state = gameObject.GetComponent<GameState>();
        state.gc = this;
        turnorder = new List<Unit>();
        EffectLibrary.Init();
    }
    void Start()
    {
        SetTurnOrder();
        foreach (Unit u in turnorder)//TEST CODE
        {
            u.SetCharacter(hr.GetCharacter("", ""));
        }
        currentUnit = turnorder[turnorder.Count - 1];
        state.uic.SetCurrentUnit(currentUnit);
    }

    public void SetTurnOrder() {
        turnorder.AddRange(state.ally.GetUnits());
        turnorder.AddRange(state.enemy.GetUnits());
        turnorder.Sort(0, turnorder.Count, compare);
    }

    public void NextTurn()
    {
        state.uic.ResetTargetState();
        turnorder.RemoveAt(turnorder.Count - 1);
        if (turnorder.Count == 0)
        {
            SetTurnOrder();
        }
        currentUnit = turnorder[turnorder.Count - 1];
        Debug.Log("Next Turn " + currentUnit.stats.GetName());
        state.uic.SetCurrentUnit(currentUnit);
    }

    public void PlayAction(Ability ability, Unit target)
    {
        AbilityResultList results = new AbilityResultList
        {
            actor = currentUnit,
            ability = ability
        };
        ability.ApplyAbility(ability.GetTargetTeam(currentUnit, state.ally, state.enemy),
            currentUnit, target, ref results);
        StartCoroutine(ExecuteAbilityTurn(results));
        
    }

    public IEnumerator ExecuteAbilityTurn(AbilityResultList results)
    {
        //lock player input
        state.ic.SetBlock(true);
        //play animation
        state.uic.action.DisplayAction(results);
        yield return new WaitForSeconds(1.0f);
        //apply delayed effects
        foreach (DelayedAbilityResult e in results.delayedEffects)
        {
            e.Apply(results.actor);
        }
        //update unit uis
        state.uic.action.HideAciton();

        //unlock player input
        state.ic.SetBlock(false);
        NextTurn(); //after finished
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextTurn();
        }
    }
}
