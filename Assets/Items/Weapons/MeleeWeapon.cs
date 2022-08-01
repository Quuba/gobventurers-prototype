using UnityEngine;

namespace Items.Weapons
{
    public class MeleeWeapon : Weapon
    {
        // public int baseDamage = 5;
        private void OnTriggerEnter(Collider other)
        {
            //Todo: ignore collision with player hurtbox
            if (other.CompareTag("Enemy"))
            {
                if (other.transform.parent.TryGetComponent(out Enemy enemy))
                {
                    WeaponAction currentAction;
                    //TODO: implement conditional attack stats
                    //if(using_primary_action)
                    currentAction = primaryAction;
                    //else if(using_secondary_action)
                    //int damage = secondaryAction.Damage;

                    
                    enemy.TakeDamage(currentAction.Damage);
                    if (currentAction.StunTime.Enabled)
                    {
                        enemy.ApplyStun(currentAction.StunTime.Value);
                    }

                    if (currentAction.Knockback.Enabled)
                    {
                        // enemy.ApplyKnockback();
                    }
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