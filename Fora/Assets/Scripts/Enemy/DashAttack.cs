using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : StateMachineBehaviour
{
    [SerializeField] private float _dashForce = 5f;
    private Rigidbody2D _rb;
    private Hog _hog;
    private AudioSource _audioSource;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rb = animator.GetComponent<Rigidbody2D>();
        _hog = animator.GetComponent<Hog>();
        _audioSource = animator.GetComponent<AudioSource>();
        _audioSource.Play();
        _rb.AddForce(_hog._directionVector * _dashForce, ForceMode2D.Impulse);
        
    }
}
