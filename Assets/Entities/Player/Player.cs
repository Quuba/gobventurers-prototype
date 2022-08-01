using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private int baseHealth;

        private int _currentHealth;
        public int CurrentHealth => _currentHealth;

        private void Awake()
        {
            _currentHealth = baseHealth;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
        }

        public void Heal(int healAmount)
        {
            _currentHealth += healAmount;
        }
    }
}

