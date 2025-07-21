using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invinsible_squirrel : StateMachineBehaviour
{
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("canHit", false);
    }
}
