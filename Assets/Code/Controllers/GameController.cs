using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Variables
    public List<Unit> turnorder;
    static CompareUnitSpeed compare = new CompareUnitSpeed();
    public Unit currentUnit;
    public GameMode mode;

    [SerializeField] Unit_Event eventUnit;

    bool playAffliction;
    //test
    public bool StartCombatImmediately;

    private void Awake()
    {
        GameState.Instance.gc = this;
        turnorder = new List<Unit>();
        mode = GameMode.exploration;
    }

    void Start(){
        if (StartCombatImmediately){
            mode = GameMode.combat;
            StartCombat(null);
        }
        else {
            GameState.Instance.ally.SetUnits(null);
            currentUnit = GameState.Instance.ally.GetUnits()[0];
            GameState.Instance.uic.SetCurrentUnit(currentUnit);
        }
    }

    public void SetEventUnit(EventDialogue e){
        if (e == null){
            eventUnit.gameObject.SetActive(false);
            return;
        }
        eventUnit.SetEvent(e);
        eventUnit.gameObject.SetActive(true);
    }

    public void SetCurrentUnit(Unit current){
        if (mode == GameMode.combat){
            return;
        }
        currentUnit = current;
    }

    public void StartCombat(EnemyGroup enemy)
    {
        mode = GameMode.combat;
        GameState.Instance.ic.StartCombat();
        GameState.Instance.uic.ToggleMenu("Combat");
        GameState.Instance.ally.SetUnits(null);
        GameState.Instance.ally.TurnLightingOn(true);
        GameState.Instance.enemy.SetUnits(enemy);
        GameState.Instance.enemy.TurnLightingOn(true);
        SetTurnOrder(true);
        currentUnit = turnorder[turnorder.Count - 1];
        GameState.Instance.uic.SetCurrentUnit(currentUnit);
        currentUnit.SetActionPips(false);
    }

    public void SetTurnOrder(bool start = false) {
        turnorder.AddRange(GameState.Instance.ally.GetUnits());
        turnorder.AddRange(GameState.Instance.enemy.GetUnits());
        if (start){
            //ApplySuprise();
        }
        turnorder.Sort(0, turnorder.Count, compare);
        GameState.Instance.ally.SetActionPips();
        GameState.Instance.enemy.SetActionPips();
    }

    public void NextTurn()
    {
        if ( mode == GameMode.exploration){ //end of curio event turn
            eventUnit.gameObject.SetActive(false);
            return;
        }
        if (GameState.Instance.ally.GetUnits().Count == 0){
            Debug.Log("FIGHT DEFEAT");
        } else if (GameState.Instance.enemy.GetUnits().Count == 0 || GameState.Instance.ally.GetUnits().Count == 0){
            GameState.Instance.ic.EndCombat();
            //bring up game end screen
            GameState.Instance.uic.OpenGameOverMenu();
        } else {
            NextCharacter();
        }
    }

    void NextCharacter()
    {
        GameState.Instance.uic.ResetTargetState();
        turnorder.RemoveAt(turnorder.Count - 1);
        if (turnorder.Count == 0)
        {
            SetTurnOrder();
            GameState.Instance.am.IncreaseAlarm();
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
            GameState.Instance.uic.SetCurrentUnit(currentUnit);
            StartCoroutine(ExecuteUnitAfflictionTurn());
        }
    }


    public void PlayEnemyTurn()
    {
        Decision d = currentUnit.ai.MakeDecision(currentUnit, GameState.Instance.ally, GameState.Instance.enemy, currentUnit.stats.GetAbilities());
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
            display = !ability.cancelDisplay,
        };
        ability.ApplyAbility(currentUnit, target, ref results);
        if (ability.metalCost > 0){
            GameState.Instance.metal.UpdateMetal(-ability.metalCost);
        }
        StartCoroutine(ExecuteAbilityTurn(results));
    }

    public void PlayEvent(Ability_Event ability, Unit trap){
        AbilityResultList results = new AbilityResultList
        {
            actor = trap,
            ability = ability,
            display = !ability.cancelDisplay,
        };
        ability.ApplyAbility(trap, currentUnit, ref results);
        StartCoroutine(ExecuteAbilityTurn(results));
    }

    public IEnumerator ExecuteAbilityTurn(AbilityResultList results)
    {
        //lock player input
        GameState.Instance.ic.SetBlock(true);
        //turn lighting teams off
        GameState.Instance.ally.TurnLightingOn(false);
        GameState.Instance.enemy.TurnLightingOn(false);
        //turn off alarm visibilty;
        GameState.Instance.am.SetAlarmVisible(false);
        //play animation
        if (results.display){
            if (!results.actor.UnitTeam.isAlly){
                GameState.Instance.uic.action.DisplayAbilityName(results);
                GameState.Instance.uic.action.ShowAbilityTargets(results);
                yield return new WaitForSeconds(0.75f);
                GameState.Instance.uic.action.HideAbilityName();
            }

            GameState.Instance.uic.action.DisplayActor(results, 0.1f);
            GameState.Instance.uic.ScaleBackground(1.5f, 0.5f);
            yield return new WaitForSeconds(0.1f);
            GameState.Instance.uic.action.DisplayTargets(results, 1.0f);
            yield return new WaitForSeconds(0.5f);
            GameState.Instance.uic.action.DisplayCounter(results);
            yield return new WaitForSeconds(1.0f);
            //update unit uis
            GameState.Instance.uic.action.HideAciton();
            GameState.Instance.uic.ScaleBackground(1.0f, 0.5f);
        }
        //turn lighting back on
        GameState.Instance.ally.TurnLightingOn(true);
        GameState.Instance.enemy.TurnLightingOn(true);
        GameState.Instance.am.SetAlarmVisible(true);
        //applies delayed effects like bleed or buffs
        List<Unit> targets = new List<Unit>();
        foreach (DelayedAbilityResult d in results.delayedEffects)
        {
            d.ApplyEffect(results.actor);
            d.Display();
            if (!targets.Contains(d.target)){
                targets.Add(d.target);
            }
        }
        //waits for all effects to be played out
        bool doneDisplay = false;
        while(!doneDisplay){
            doneDisplay = true;
            foreach(Unit u in targets){
                if (!u.IsFinishedPopupText()){
                    doneDisplay = false;
                }
            }
            yield return new WaitForFixedUpdate();
        }
        //applies stress
        foreach (DelayedAbilityResult d in results.stress){
            d.ApplyEffect(results.actor);
            d.Display();
            while (playAffliction){
                yield return new WaitForSeconds(GameState.Instance.uic.affliction.GetDuration());
            }
            yield return new WaitForSeconds(0.75f);
        }
        //unlock player input
        GameState.Instance.ic.SetBlock(false);
        NextTurn(); //after finished
        yield return null;
    }

    public IEnumerator ExecuteInitialAfflictionTurn(Unit afflicted){
        playAffliction = true;
        GameState.Instance.uic.affliction.ShowResolve(afflicted);
        yield return new WaitForSeconds(1.5f);
        GameState.Instance.uic.affliction.DisplayAffliction(afflicted);
        yield return new WaitForSeconds(GameState.Instance.uic.affliction.GetDuration());
        playAffliction = false;
    }

    public void ApplySuprise(){
        int allySuprise = Random.Range(0, GameState.Instance.ally.supriseChance);
        int enemySuprise = Random.Range(0, GameState.Instance.enemy.supriseChance);
        const int supriseThreshold = 50;
        if (allySuprise >= enemySuprise && allySuprise > supriseThreshold){
            foreach (Unit u in GameState.Instance.enemy.GetUnits()){
                u.stats.ApplyDelayedEffect(EffectLibrary.GetEffect("Suprise") as StatusEffect);
            }
        } else if (enemySuprise > supriseThreshold) {
            foreach (Unit u in GameState.Instance.ally.GetUnits()){
                u.stats.ApplyDelayedEffect(EffectLibrary.GetEffect("Suprise") as StatusEffect);
            }
        }
    }

    IEnumerator ExecuteUnitAfflictionTurn(){
        if (currentUnit.stats.modifiers.Affliction != null){
            if (currentUnit.stats.modifiers.Affliction.AfflictionTurnChance()){
                GameState.Instance.ic.SetBlock(true);
                yield return StartCoroutine(currentUnit.stats.modifiers.Affliction.StressTeam(currentUnit));
            }
        }
        GameState.Instance.ic.SetBlock(false);
    }
    
}
