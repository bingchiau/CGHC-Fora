using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    [Header("Settings")]
    [Range(1, 20)]
    [SerializeField] private float _speed = 15f;

    [Range(1f, 10f)]
    [SerializeField] private float _lifeTime = 2f;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, _lifeTime);
    }

    private void FixedUpdate()
    {
        _rb.velocity = transform.up * _speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }
}
