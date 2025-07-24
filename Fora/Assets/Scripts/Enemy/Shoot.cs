using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : StateMachineBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject _nuts;
    [SerializeField] private float _cooldown;

    private Transform _spawnPos;
    private AudioSource _audioSource;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _spawnPos = animator.transform.GetChild(0);
        _audioSource = animator.GetComponent<AudioSource>();
        _audioSource.Play();
        Instantiate(_nuts, _spawnPos.position, Quaternion.identity);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("attackCountdown", _cooldown);
    }
}
