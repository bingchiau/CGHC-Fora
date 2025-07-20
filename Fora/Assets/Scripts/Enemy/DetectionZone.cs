using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> DetectedColliders = new List<Collider2D>();
    Collider2D _col;

    private void Start()
    {
        _col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DetectedColliders.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        DetectedColliders.Remove(collision);
    }
}
