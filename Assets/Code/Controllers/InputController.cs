using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public enum InputState
    {
        PlayingAction,
        Normal,
        Block,
    }

    public GameState state;
    [SerializeField] InputState inputState;
    Ability currentAbility;
   

    // Start is called before the first frame update
    void Awake()
    {
        state = gameObject.GetComponent<GameState>();
        state.ic = this;
        inputState = InputState.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectCharacter(Unit c)
    {
        //TODO : check for aoe abilities
        if (inputState == InputState.PlayingAction)
        {
            Debug.Log("Checking Play");
            if (c.targetedState == TargetedState.Targeted)
            {
                Debug.Log("Checking Play");
                state.gc.PlayAction(currentAbility, c);
            }
            else
            {
                state.uic.DeselectAbility();
            }
        }
        else if (inputState != InputState.Block)
        {
            Debug.Log("Switching Selected Character");
            state.uic.SetCurrentUnit(c);
            inputState = InputState.Normal;
            currentAbility = null;
        }
    }

    public bool GetBlocked()
    {
        return inputState == InputState.Block;
    }

    public void SetBlock(bool b)
    {
        inputState = b ? InputState.Block : InputState.Normal;
    }

    public bool SelectAbility(Ability a)
    {
        //TODO : check current turn
        if (inputState != InputState.Block)
        {
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
            inputState = InputState.Normal;
        }
    }
}
