using UnityEngine;

public class FireballAutoDestroy : MonoBehaviour
{
    [Header("Lifetime Settings")]
    public float lifetime = 2.5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
