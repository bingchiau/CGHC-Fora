using UnityEngine;

public abstract class BossController : MonoBehaviour
{
    [Header("General Boss Settings")]
    public float health = 100f;
    public float attackCooldown = 3f;

    protected Transform player;
    private float lastAttackTime;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        // No auto-attack — leave attack logic to subclasses
    }

    protected bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }

    protected void MarkAttackUsed()
    {
        lastAttackTime = Time.time;
    }

    protected abstract void PerformAttack();

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
