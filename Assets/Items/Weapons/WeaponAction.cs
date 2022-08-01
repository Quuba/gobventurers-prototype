using System;
using UnityEngine;

namespace Items.Weapons
{
    [Serializable]
    public class WeaponAction
    {
        public Optional<float> StunTime;
        public Optional<float> Knockback;
        public int Damage = 1;

        public bool canBeCharged = false;
        public void Use()
        {
            Debug.Log("weapon action is not implemented");
        }
    }
}