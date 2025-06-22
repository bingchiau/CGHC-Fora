using UnityEngine;

public class DroppingFireball : MonoBehaviour
{
    public Transform spawnPoint;
    public LayerMask mapLayerMask;
    public GameObject playerObject;
    public float dropDelay = 0.5f;
    public float delayVelocity = 0f;
    public float delayDrag = 0f;
    public float delayGravity = 0f;

    private Rigidbody2D rb;
    private bool isDelaying = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("DroppingFireball needs a Rigidbody2D!");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDelaying) return;

        // - MAP Collision -
        if (((1 << other.gameObject.layer) & mapLayerMask) != 0)
        {
            ResetAndDelayDrop();
        }

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // - PLAYER Collision -     !!ADD DAMAGE TO PLAYER!!
        if (other.gameObject == playerObject)
        {
            Debug.Log("Hit the player.");
            // TO-DO: Add damage here if needed later
            ResetAndDelayDrop();
        }
    }


    void ResetAndDelayDrop()
    {
        // Immediately teleport back
        transform.position = spawnPoint.position;

        // Stop motion and disable gravity
        rb.velocity = Vector2.zero;
        rb.angularVelocity = delayVelocity;
        rb.angularDrag = delayDrag;
        rb.gravityScale = delayGravity;

        // Start delay
        isDelaying = true;
        Invoke(nameof(EnableGravity), dropDelay);
    }

    void EnableGravity()
    {
        rb.gravityScale = 1f; // or whatever your default gravity was
        isDelaying = false;
    }
}
