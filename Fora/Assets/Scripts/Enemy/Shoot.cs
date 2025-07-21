using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : StateMachineBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject _nuts;
    [SerializeField] private float _cooldown;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Instantiate(_nuts, animator.transform.position, Quaternion.identity);
        animator.SetBool("canHit", true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("attackCountdown", _cooldown);
    }
}
