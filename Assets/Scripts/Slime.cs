﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Slime : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask movementCollision;
    public Transform movePoint;
    private Vector3 lastGeneratedVector;
    private SpriteRenderer spriteRenderer;

    //public MovementController movCon;

    // private SpriteRenderer spriteRenderer;
    //
    // public enum Look
    // {
    //     UP,
    //     LEFT,
    //     DOWN,
    //     RIGHT
    // }
    // public Look lookDirection = Look.LEFT;

    void Start()
    {
        //movCon = GameObject.FindGameObjectWithTag("MovementController").GetComponent<MovementController>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        movePoint.parent = null;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = GenerateColor();
    }

    void Update()
    {
        UpdateMovement();
    }
    
    private void UpdateMovement()
    {

        Debug.DrawLine(transform.position, movePoint.position, Color.green, 5f);

        if (Vector3.Distance(transform.position, movePoint.position) > 0)
        {
            if (!Physics2D.OverlapCircle(movePoint.position, 0.2f, movementCollision))
            {
                transform.position =
                    Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                movePoint.position = transform.position; // Could not move there, so don't try
            }
        }
        
    }

    private void OnEnable()
    {
        MovementEventManager.OnPlayerMove += Move;
    }

    private void OnDisable()
    {
        MovementEventManager.OnPlayerMove -= Move;
    }

    public void Kill()
    {
        EnemyEventManager.TriggerEnemyKilled(gameObject);
    }

    private void Move()
    {
        lastGeneratedVector = GenerateRandomDirection();
        movePoint.position += lastGeneratedVector;
    }

    private Vector3 GenerateRandomDirection()
    {
        int dice = Random.Range(1, 5);

        switch (dice)
        {
            case 1:
                return Vector3.left;
            case 2:
                return Vector3.right;
            case 3:
                return Vector3.up;
            case 4:
                return Vector3.down;
            default:
                return Vector3.zero;
        }
    }

    private Color GenerateColor()
    {
        int dice = Random.Range(1, 4);
        
        switch (dice)
        {
            case 1:
                Color gcolor = new Color();
                ColorUtility.TryParseHtmlString ("#8cba80", out gcolor);
                return gcolor;
            case 2:
                Color bcolor = new Color();
                ColorUtility.TryParseHtmlString ("#639bff", out bcolor);
                return bcolor;
            default:
                Color scolor = new Color();
                ColorUtility.TryParseHtmlString ("#cf995f", out scolor);
                return scolor;
        }
    }

    private void OnDestroy()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Play("Hit");   
        }
    }
}
