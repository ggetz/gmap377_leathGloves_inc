﻿using UnityEngine;

public class Weapon: MonoBehaviour
{
    public float damage, speed, ammo;
	public string targetTag = "Enemy";
    
    /// <summary>
    /// Triggered when the bullet collides with anything
    /// </summary>
    /// <param name="col">The object it collides with</param>
    void OnCollisionEnter(Collision col)
    {
        SystemLogger.write("Bullet collision");
        // If it's an enemy
        if (col.gameObject.CompareTag(this.targetTag))
        {
            // Try and find an EnemyHealth script on the gameobject hit.
            EnemyStats enemyHealth = col.gameObject.GetComponentInParent<EnemyStats>();

			// if not in parent
			if(enemyHealth == null)
			{
				enemyHealth = col.gameObject.GetComponent<EnemyStats>();
			}

            // If the EnemyHealth component exists...
            if (enemyHealth != null)
            {
                // ... the enemy should take damage.
                enemyHealth.TakeDamage((int)this.damage);

                if(PowerUpManager.Instance.Powerups["DamageUp"].IsActive)
                {
                    enemyHealth.TakeDamage((int)PowerUpManager.Instance.PowerIncrease);
                }
            }
        }
    }
}
