using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    [Header("Settings")]
    [Range(1, 20)]
    [SerializeField] private float _speed = 10f;

    [Range(1f, 10f)]
    [SerializeField] private float _lifeTime = 1.5f;

    [SerializeField] private bool _enemyBullet;

    private GameObject _player;
    public Rigidbody2D _rb;

    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player");

        if (_enemyBullet && _player != null)
        {
            Vector3 direction = _player.transform.position - transform.position;
            _rb.velocity = new Vector2(direction.x, direction.y).normalized * _speed;
        }
        else if (!_enemyBullet)
        {
            _rb.velocity = transform.up * _speed;
        }

        Destroy(gameObject, _lifeTime);
    }

    private void FixedUpdate()
    {
        float angle = Mathf.Atan2(_rb.velocity.y, _rb.velocity.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DestroyThis(collision);
    }

    public virtual void DestroyThis(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            Destroy(gameObject);
        }
    }
}
