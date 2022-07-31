using System;
using UnityEngine;

namespace Items.Weapons
{
    [Serializable]
    public class WeaponAction
    {
        public bool canBeCharged = false;
        [SerializeField] private Animation actionAnimation;
        public void Use()
        {
            Debug.Log("weapon action is not implemented");
        }
    }
}