using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int currentHealth;
    public int Health => currentHealth;
    [SerializeField] private float invincibilityTime = 0.5f;
    private float invincibilityTimer = 0.5f;

    private bool isInvincible;

    [Header("Debug")]
    public bool showHealthBar;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
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
    }
}