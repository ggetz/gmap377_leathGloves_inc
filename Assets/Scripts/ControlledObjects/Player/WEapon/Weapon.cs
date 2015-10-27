﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon: MonoBehaviour
{
    public float Damage, Speed;

    /// <summary>
    /// Triggered when the bullet collides with anything
    /// </summary>
    /// <param name="col">The object it collides with</param>
    void OnCollisionEnter(Collision col)
    {
        // If it's an enemy
        if (col.gameObject.tag == "Enemy")
        {
            // Try and find an EnemyHealth script on the gameobject hit.
            EnemyHealth enemyHealth = col.gameObject.GetComponent<EnemyHealth>();

            // If the EnemyHealth component exist...
            if (enemyHealth != null)
            {
                // ... the enemy should take damage.
                enemyHealth.TakeDamage((int)this.Damage);
            }

        }

    }
}
