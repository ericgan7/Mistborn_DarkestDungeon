using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Increment : StateMachineBehaviour
{
    float velocity;
    float speed;
    List<float> fillLevels;
    int currentRadial;
    int alarmLevel;
    Image radial;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        Alarm controller = animator.gameObject.GetComponent<Alarm>();
        velocity = 0f;
        speed = controller.speed;
        fillLevels = controller.fillLevels;
        currentRadial = controller.currentRadial;
        alarmLevel = controller.alarmLevel;
        radial = controller.radial;
        animator.ResetTrigger("increment");
        if (currentRadial == fillLevels.Count)
        {
            currentRadial = 0;
            alarmLevel += 1;
            animator.SetTrigger("cycle");
        }
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float diff = fillLevels[currentRadial] - radial.fillAmount;
        if (diff > 0.001)
        {
            Debug.Log(fillLevels[currentRadial]);
            radial.fillAmount = Mathf.SmoothDamp(radial.fillAmount,
                fillLevels[currentRadial],
                ref velocity,
                speed
                );
            diff = fillLevels[currentRadial] - radial.fillAmount;
        } 
        else
        {
            animator.SetTrigger("finished");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
