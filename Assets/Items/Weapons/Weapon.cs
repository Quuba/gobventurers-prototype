using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Items.Weapons
{
    [RequireComponent(typeof(Animator))]
    public class Weapon : MonoBehaviour
    {
        private Animator _animator;
        public bool hasSecondaryAction;
        public WeaponAction primaryAction;
        [CanBeNull] public WeaponAction secondaryAction;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void UsePrimaryAction()
        {
            _animator.SetTrigger("primaryAction");
        }

        public void UseSecondaryAction()
        {
            
        }
    }
}