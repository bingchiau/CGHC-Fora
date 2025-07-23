using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConditions : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int horizontalRayCount = 4;
    [SerializeField] private LayerMask _collideWith;

    private BoxCollider2D _boxCollider2D;

    private Vector2 _boundsBottomLeft;
    private Vector2 _boundsBottomRight;
    private Vector2 _boundsTopLeft;
    private Vector2 _boundsTopRight;
    private float _skin = 0.05f;

    private float _boundsWidth;
    private float _boundsHeight;

    // Conditions
    public bool IsCollidingWall;


    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        SetRayOrigins();
        HorizontalCollision((int)transform.localScale.x);
    }

    // Calculate ray based on our collider
    private void SetRayOrigins()
    {
        Bounds playerBounds = _boxCollider2D.bounds;

        _boundsBottomLeft = new Vector2(playerBounds.min.x, playerBounds.min.y);
        _boundsBottomRight = new Vector2(playerBounds.max.x, playerBounds.min.y);
        _boundsTopLeft = new Vector2(playerBounds.min.x, playerBounds.max.y);
        _boundsTopRight = new Vector2(playerBounds.max.x, playerBounds.max.y);

        _boundsHeight = Vector2.Distance(_boundsBottomLeft, _boundsTopLeft);
        _boundsWidth = Vector2.Distance(_boundsBottomLeft, _boundsBottomRight);
    }


    private void HorizontalCollision(int direction)
    {
        Vector2 rayHorizontalBottom = (_boundsBottomLeft + _boundsBottomRight) / 2f;
        Vector2 rayHorizontalTop = (_boundsTopLeft + _boundsTopRight) / 2f;
        rayHorizontalBottom += (Vector2)transform.up * _skin;
        rayHorizontalTop -= (Vector2)transform.up * _skin;

        float rayLenght = _boundsWidth / 2f + _skin * 2f;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = Vector2.Lerp(rayHorizontalBottom, rayHorizontalTop, (float)i / (horizontalRayCount - 1));
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction * transform.right, rayLenght, _collideWith);
            Debug.DrawRay(rayOrigin, transform.right * rayLenght * direction, Color.cyan); 

            if (hit)
            {
                IsCollidingWall = true;
            }
            else
            {
                IsCollidingWall = false;
            }
        }
    }

}
