using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public enum InputState
    {
        PlayingAction,
        Event,
        Block,
        Combat,
    }

    public GameState state;
    [SerializeField] InputState inputState;
    Ability currentAbility;

    public void Init(){
        GameState.Instance.ic = this;
        GameEvents.current.onSelectAbility += SelectAbility;
    }

    private void OnDestroy() {
        GameEvents.current.onSelectAbility -= SelectAbility;
    }
   

    // Start is called before the first frame update
    void Awake()
    {
        Init();
        inputState = InputState.Event;
    }

    public void StartCombat(){
        inputState = InputState.Combat;
    }

    public void EndCombat(){
        inputState = InputState.Event;
    }

    public void SelectCharacter(Unit c)
    {
        if (inputState == InputState.PlayingAction)
        {
            if (c.targetedState == TargetedState.Targeted)
            {
                GameState.Instance.gc.PlayAction(currentAbility, c);
            }
            else
            {
                GameState.Instance.uic.DeselectAbility();
            }
        } else if (inputState == InputState.Event){
            GameState.Instance.uic.SetCurrentUnit(c);
            GameState.Instance.gc.SetCurrentUnit(c);
        }
    }

    public bool GetBlocked()
    {
        return inputState == InputState.Block;
    }

    public void SetBlock(bool b)
    {
        inputState = b ? InputState.Block : InputState.Combat;
    }

    public void SelectAbility(Ability ability)
    {
        if (inputState != InputState.Block)
        {
            // unusable outside of comabt
            if (GameState.Instance.gc.mode != GameMode.combat && !ability.usableOutOfComabat){
                return;
            }
            if (ability == null){
                DeselectAbility();
                return;
            }
            // set ability
            inputState = InputState.PlayingAction;
            currentAbility = ability;
        }
    }

    public void DeselectAbility()
    {
        if (inputState != InputState.Block)
        {
            inputState = InputState.Combat;
        }
    }
}
