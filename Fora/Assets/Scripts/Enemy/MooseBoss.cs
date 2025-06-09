using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(AIDestinationSetter))]
public class MooseBoss : BossController
{
    [Header("A* Movement")]
    public float chargeTriggerDistance = 3f;

    [Header("Charge Settings")]
    public float chargeSpeed = 12f;
    public float chargeDuration = 1.2f;
    public float stunDuration = 0.6f;

    private AIPath aiPath;
    private AIDestinationSetter destinationSetter;
    private Rigidbody2D rb;
    private bool isCharging = false;

    protected override void Start()
    {
        base.Start();

        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        rb = GetComponent<Rigidbody2D>();

        if (destinationSetter != null)
        {
            destinationSetter.target = player;
        }
    }

    protected override void Update()
    {
        base.Update();

        float distance = Vector2.Distance(transform.position, player.position);
        if (!isCharging && distance < chargeTriggerDistance)
        {
            if (CanAttack())
            {
                Debug.Log("Moose starts charging!");
                StartCoroutine(ChargeAtPlayer());
            }
        }
    }

    protected override void PerformAttack()
    {
        MarkAttackUsed();  // cooldown reset
    }

    private IEnumerator ChargeAtPlayer()
    {
        PerformAttack();

        isCharging = true;
        aiPath.canMove = false;
        aiPath.enabled = false;

        // Stop current motion
        rb.velocity = Vector2.zero;

        // Direction toward player
        Vector2 direction = (player.position - transform.position).normalized;
        if (direction.sqrMagnitude < 0.1f)
        {
            direction = Vector2.right;  // fallback direction
        }

        rb.velocity = direction * chargeSpeed;
        Debug.Log("Charging in direction: " + direction);
        Debug.DrawRay(transform.position, direction * 2, Color.red, 1f);

        yield return new WaitForSeconds(chargeDuration);

        rb.velocity = Vector2.zero;
        Debug.Log("Charge complete. Stunned...");

        yield return new WaitForSeconds(stunDuration);

        aiPath.enabled = true;
        aiPath.canMove = true;
        isCharging = false;
    }
}
