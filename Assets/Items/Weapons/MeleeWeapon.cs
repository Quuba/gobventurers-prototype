using UnityEngine;

namespace Items.Weapons
{
    public class MeleeWeapon : Weapon
    {

        public int baseDamage = 5;
        private void OnTriggerEnter(Collider other)
        {
            //Todo: ignore collision with player hurtbox
            if (other.CompareTag("Enemy"))
            {
                if (other.transform.parent.TryGetComponent(out Enemy enemy))
                {
                    enemy.TakeDamage(baseDamage);
                }
                else
                {
                    Debug.LogError("Could not find enemy script on enemy tagged gameobject");
                }
            }
            // Debug.Log(other.gameObject.name);
        }
    }
}