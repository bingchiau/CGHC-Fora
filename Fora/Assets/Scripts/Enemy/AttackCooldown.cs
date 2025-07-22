using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCooldown : MonoBehaviour
{
    private Animator _animator;

    public float AttackCountdown {
        get
        {
            return _animator.GetFloat("attackCountdown");
        }
        private set
        {
            _animator.SetFloat("attackCountdown", Mathf.Max(value, 0));
        } 
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (AttackCountdown > 0)
        {
            AttackCountdown -= Time.deltaTime;
        }
    }
}
