using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int baseHealth;
    [SerializeField] private int currentHealth;
    public int Health => currentHealth;
    [SerializeField] private float invincibilityTime = 0.5f;
    private float invincibilityTimer = 0.5f;

    private bool isInvincible;

    private EnemyState currentState;
    public EnemyState GetEnemyState => currentState;
    
    public event Action<EnemyState> EnemyStateChanged;

    private float stunTimer;

    [Header("Debug")]
    public bool showHealthBar;

    private void Awake()
    {
        currentState = EnemyState.Default;
        currentHealth = baseHealth;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Default:
                break;
            case EnemyState.Stunned:
                if (stunTimer <= 0)
                {
                    SetEnemyState(EnemyState.Default);
                }
                else
                {
                    stunTimer -= Time.deltaTime;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        if (isInvincible)
        {
            invincibilityTimer += Time.deltaTime;
            if (invincibilityTimer >= invincibilityTime)
            {
                isInvincible = false;
                invincibilityTimer = 0f;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            Debug.Log($"ouch, I took {damage} damage");
            isInvincible = true;
        }
        
        Stun(5f);
    }

    public void Stun(float stunTime)
    {
        stunTimer = stunTime;
        SetEnemyState(EnemyState.Stunned);
    }

    public void SetEnemyState(EnemyState newState)
    {
        currentState = newState;
        EnemyStateChanged?.Invoke(newState);
    }
}