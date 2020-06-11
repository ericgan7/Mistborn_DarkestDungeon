using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Variables
    public GameState state;

    public List<Unit> turnorder;
    static CompareUnitSpeed compare = new CompareUnitSpeed();
    public Unit currentUnit;
    public GameMode mode;
    //test
    public bool StartCombatImmediately;

    private void Awake()
    {
        state = FindObjectOfType<GameState>();
        state.gc = this;
        turnorder = new List<Unit>();
        mode = GameMode.exploration;
    }

    void Start(){
        if (StartCombatImmediately){
            mode = GameMode.combat;
            StartCombat(null);
        }
        else {
            state.ally.SetUnits(null);
            currentUnit = state.ally.GetUnits()[0];
            state.uic.SetCurrentUnit(currentUnit);
        }
    }

    public void SetCurrentUnit(Unit current){
        if (mode == GameMode.combat){
            return;
        }
        currentUnit = current;
    }

    public void StartCombat(EnemyGroup enemy)
    {
        Debug.Log("START COMBAT");
        state.ic.StartCombat();
        state.uic.ToggleMenu("Ability");
        state.ally.SetUnits(null);
        state.enemy.SetUnits(enemy);
        SetTurnOrder(true);
        currentUnit = turnorder[turnorder.Count - 1];
        state.uic.SetCurrentUnit(currentUnit);
        currentUnit.SetActionPips(false);
    }

    public void SetTurnOrder(bool start = false) {
        turnorder.AddRange(state.ally.GetUnits());
        turnorder.AddRange(state.enemy.GetUnits());
        if (start){
            ApplySuprise();
        }
        turnorder.Sort(0, turnorder.Count, compare);
        state.ally.SetActionPips();
        state.enemy.SetActionPips();
    }

    public void NextTurn()
    {
        if (state.ally.GetUnits().Count == 0){
            Debug.Log("FIGHT DEFEAT");
        } else if (state.enemy.GetUnits().Count == 0){
            state.ic.EndCombat();
            //bring up game end screen
        } else {
            NextCharacter();
        }
    }

    void NextCharacter()
    {
        state.uic.ResetTargetState();
        turnorder.RemoveAt(turnorder.Count - 1);
        if (turnorder.Count == 0)
        {
            SetTurnOrder();
            state.am.IncreaseAlarm();
        }
        currentUnit = turnorder[turnorder.Count - 1];
        currentUnit.SetActionPips(false);
        if (!currentUnit.stats.OnTurnStart()){
            NextTurn();
        }
        else if (!currentUnit.UnitTeam.isAlly)
        {
            PlayEnemyTurn();
        }else
        {
            state.uic.SetCurrentUnit(currentUnit);
            StartCoroutine(ExecuteAfflictionTurn());
        }
    }


    public void PlayEnemyTurn()
    {
        Decision d = currentUnit.ai.MakeDecision(currentUnit, state.ally, state.enemy, currentUnit.stats.GetAbilities());
        StartCoroutine(PlayEnemySelection(d));
    }

    public IEnumerator PlayEnemySelection(Decision decision)
    {
        decision.target.SetTargetState(TargetedState.Targeted);
        //TODO display ability name
        yield return new WaitForSeconds(1.0f);
        PlayAction(decision.ability, decision.target);
    }


    public void PlayAction(Ability ability, Unit target)
    {
        AbilityResultList results = new AbilityResultList
        {
            actor = currentUnit,
            ability = ability,
            display = true,
        };
        ability.ApplyAbility(ability.GetTargetTeam(currentUnit, state.ally, state.enemy),
            currentUnit, target, ref results);
        if (ability.metalCost > 0){
            state.am.UpdateMetal(-ability.metalCost);
        }
        Debug.Log("Executing ability");
        StartCoroutine(ExecuteAbilityTurn(results));
    }

    public IEnumerator ExecuteAbilityTurn(AbilityResultList results)
    {
        //lock player input
        state.ic.SetBlock(true);
        //play animation
        if (!results.ability.cancelDisplay){
            if (!results.actor.UnitTeam.isAlly){
                state.uic.action.DisplayAbilityName(results);
                state.uic.action.ShowAbilityTargets(results);
                yield return new WaitForSeconds(0.75f);
                state.uic.action.HideAbilityName();
            }

            state.uic.action.DisplayActor(results, 0.2f);
            state.uic.ScaleBackground(1.5f, 0.5f);
            yield return new WaitForSeconds(0.1f);
            state.uic.action.DisplayTargets(results);
            yield return new WaitForSeconds(0.5f);
            state.uic.action.DisplayCounter(results);
            yield return new WaitForSeconds(1.0f);
            //update unit uis
            state.uic.action.HideAciton();
            state.uic.ScaleBackground(1.0f, 0.5f);
        }
        for (int i = 0; i < results.delayedEffects.Count; ++i)
        {
            results.delayedEffects[i].Apply(results.actor);
            results.delayedEffects[i].Display(0.5f * i);
        }
        yield return new WaitForSeconds(0.5f); // play out all delayed effect displays
        //unlock player input
        state.ic.SetBlock(false);
        NextTurn(); //after finished
        yield return null;
    }

    public void ApplySuprise(){
        int allySuprise = Random.Range(0, state.ally.supriseChance);
        int enemySuprise = Random.Range(0, state.enemy.supriseChance);
        const int supriseThreshold = 50;
        if (allySuprise >= enemySuprise && allySuprise > supriseThreshold){
            foreach (Unit u in state.enemy.GetUnits()){
                u.stats.ApplyDelayedEffect(EffectLibrary.GetEffect("Suprise") as StatusEffect);
            }
        } else if (enemySuprise > supriseThreshold) {
            foreach (Unit u in state.ally.GetUnits()){
                u.stats.ApplyDelayedEffect(EffectLibrary.GetEffect("Suprise") as StatusEffect);
            }
        }
    }

    IEnumerator ExecuteAfflictionTurn(){
        if (currentUnit.stats.modifiers.Affliction != null){
            if (currentUnit.stats.modifiers.Affliction.AfflictionTurnChance()){
                state.ic.SetBlock(true);
                yield return StartCoroutine(currentUnit.stats.modifiers.Affliction.StressTeam(currentUnit));
            }
        }
        state.ic.SetBlock(false);
    }
    
}
