using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceSurface : MonoBehaviour
{
    public static Action<float> OnBounce;

    private void Start()
    {
        //...
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //..
    }
}
