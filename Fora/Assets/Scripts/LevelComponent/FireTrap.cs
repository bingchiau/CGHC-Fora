using System.Collections;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    public int damagePerTick = 1;
    public float tickInterval = 1f;

    private Coroutine damageCoroutine;

    private bool isDisabled = false;
    public float disableDuration = 3f;

    private Collider2D col;
    private Animator anim;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDisabled) return;

        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            damageCoroutine = StartCoroutine(DamageOverTime(playerStats));
        }
        else if (collision.CompareTag("WaterDroplet"))
        {
            StartCoroutine(DisableFire());
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();

        if (playerStats != null && damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    private IEnumerator DamageOverTime(PlayerStats player)
    {
        while (true)
        {
            player.TakeDamage(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }
    }

    private IEnumerator DisableFire()
    {
        isDisabled = true;

        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }

        anim.SetTrigger("FireOut");
        col.enabled = false;

        yield return new WaitForSeconds(disableDuration);

        anim.SetTrigger("Ignite");
        col.enabled = true;

        isDisabled = false;
    }
}
