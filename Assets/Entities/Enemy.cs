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
    private float _invincibilityTimer = 0.5f;

    private bool _isInvincible;

    private EnemyState _currentState;
    public EnemyState GetEnemyState => _currentState;
    
    public event Action<EnemyState> EnemyStateChanged;
    [SerializeField] private Animator animator;

    private float _stunTimer;

    [Header("Debug")]
    public bool showHealthBar;

    private void Awake()
    {
        _currentState = EnemyState.Default;
        currentHealth = baseHealth;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case EnemyState.Default:
                break;
            case EnemyState.Stunned:
                if (_stunTimer <= 0)
                {
                    SetEnemyState(EnemyState.Default);
                }
                else
                {
                    _stunTimer -= Time.deltaTime;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        if (_isInvincible)
        {
            _invincibilityTimer += Time.deltaTime;
            if (_invincibilityTimer >= invincibilityTime)
            {
                _isInvincible = false;
                _invincibilityTimer = 0f;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!_isInvincible)
        {
            currentHealth -= damage;
            Debug.Log($"ouch, I took {damage} damage");
            if (currentHealth <= 0)
            {
                Die();
            }
            
            _isInvincible = true;
            animator.SetTrigger(GetAnimatorParameterValue(AnimatorParameter.HurtTrigger));
        }
    }

    private void Die()
    {
        //author: Jonasz Kule
        Debug.Log("CURSE YOU PERRY THE PLATYPUS");
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().AddForce(Vector3.up * 30, ForceMode.VelocityChange);
        Destroy(gameObject, 1.5f);
    }

    public void ApplyStun(float stunTime)
    {
        _stunTimer = stunTime;
        SetEnemyState(EnemyState.Stunned);
    }

    public void ApplyKnockback(Vector3 direction, float force)
    {
        
    }

    public void SetEnemyState(EnemyState newState)
    {
        _currentState = newState;
        EnemyStateChanged?.Invoke(newState);
    }
    private enum AnimatorParameter
    {
        HurtTrigger,
        
    }
    Dictionary<AnimatorParameter, string> animatorParamDict = new Dictionary<AnimatorParameter, string>()
    {
        {AnimatorParameter.HurtTrigger, "hurt"}
    };
    
    private string GetAnimatorParameterValue(AnimatorParameter parameter)
    {
        
        return animatorParamDict[parameter];
    }
}