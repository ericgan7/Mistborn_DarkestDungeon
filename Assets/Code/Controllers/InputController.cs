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
   

    // Start is called before the first frame update
    void Awake()
    {
        state = FindObjectOfType<GameState>();
        state.ic = this;
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
                state.gc.PlayAction(currentAbility, c);
            }
            else
            {
                state.uic.DeselectAbility();
            }
        } else if (inputState == InputState.Event){
            state.uic.SetCurrentUnit(c);
            state.gc.SetCurrentUnit(c);
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

    public bool SelectAbility(Ability a)
    {
        //TODO : check current turn
        if (inputState != InputState.Block)
        {
            if (state.gc.mode != GameMode.combat && !a.usableOutOfComabat){
                return false;
            }
            inputState = InputState.PlayingAction;
            currentAbility = a;
            return true;
        }
        return false;
    }

    public void DeselectAbility()
    {
        if (inputState != InputState.Block)
        {
            inputState = InputState.Combat;
        }
    }
}
