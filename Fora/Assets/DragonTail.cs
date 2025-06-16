using UnityEngine;

public class DragonTail : MonoBehaviour
{
    [Header("Tail Segments")]
    public Transform[] segments; // From base to tip

    [Header("Target")]
    public Transform player;

    [Header("Tail Settings")]
    public float segmentLength = 0.5f;
    public int iterations = 5;

    void LateUpdate()
    {
        if (player == null) return;

        // 1) Start: tip points to player
        Vector3 targetPosition = player.position;

        // 2) Backwards: adjust each segment towards its child
        segments[segments.Length - 1].position = targetPosition;
        for (int i = segments.Length - 2; i >= 0; i--)
        {
            Transform child = segments[i + 1];
            Transform parent = segments[i];

            Vector3 dir = (parent.position - child.position).normalized;
            parent.position = child.position + dir * segmentLength;
        }

        // 3) Forward pass: keep base fixed, propagate positions forward
        segments[0].position = transform.position; // Base is fixed to dragon
        for (int i = 1; i < segments.Length; i++)
        {
            Transform prev = segments[i - 1];
            Transform curr = segments[i];

            Vector3 dir = (curr.position - prev.position).normalized;
            curr.position = prev.position + dir * segmentLength;
        }
    }
}
