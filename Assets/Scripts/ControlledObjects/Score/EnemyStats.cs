﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyStats : MonoBehaviour
{
    public int startingHealth = 100;        // The amount of health the enemy starts the game with.
    public int currentHealth;               // The current health the enemy has.

    public int BoltDropAmount = 5;
    public GameObject Bolt;
    public int scoreValue = 100;           // The amount added to the player's score when the enemy dies.
    public int DropChance = 25;
    public float offset = 1;
    public float Scatter = 0.01f;
    public int ScatterMin = -5;
    public int ScatterMax = 5;
    public GameObject[] Drops;
    public GameObject HitEffect;
    public GameObject DeathEffect;

    public bool Flashing = false;
    public Material HitFlashMaterial;
    public float HitFlashLength;
    private List<Material[]> _childMaterials = new List<Material[]>();


    private System.Random drop;
    bool isDead;                           // Whether the enemy is dead.

    void Awake()
    {

        this.drop = new System.Random();

        // Setting the current health when the enemy first spawns.
        currentHealth = startingHealth;
    }

    void Update()
    {
        // If the enemy 
        if (isDead)
        {
            // Increase the score by the enemy's score value.
            ScoreManager.Instance.IncreaseScore(scoreValue);
            DestroySelf();
        }

    }

    public void TakeDamage(int amount)
    {
        // If the enemy is dead...
        if (isDead)
        {
            // ... no need to take damage so exit the function.
            return;
        }
            
        // Reduce the current health by the amount of damage sustained.
        currentHealth -= amount;
        if (HitEffect)
        {
            Instantiate(this.HitEffect, this.transform.position, Quaternion.identity);
            if (!Flashing) StartCoroutine(HitFlash());
        }

        // If the current health is less than or equal to zero...
        if (currentHealth <= 0)
        {
            // ... the enemy is dead.
            Death();
        }
    }

    IEnumerator HitFlash() {
        Flashing = true;
        Material[] hitFlashList = new Material[1];
        hitFlashList[0] = HitFlashMaterial;

        _childMaterials.Clear();
        HitFlashMeshes(transform, hitFlashList);

        yield return new WaitForSeconds(HitFlashLength);

        ReplaceMeshes(transform, 0);
        _childMaterials.Clear();
        Flashing = false;
    }

    void HitFlashMeshes(Transform root, Material[] hitFlashMaterials) {
        if (root.GetComponents<MeshRenderer>().Length != 0) {
            MeshRenderer meshRenderer = root.GetComponent<MeshRenderer>();
            List<Material> replacement = new List<Material>();
            _childMaterials.Add(meshRenderer.materials);
            foreach (Material m in meshRenderer.materials) replacement.Add(HitFlashMaterial);
            meshRenderer.materials = replacement.ToArray();
        }
        else if (root.GetComponents<SkinnedMeshRenderer>().Length != 0) {
            SkinnedMeshRenderer meshRenderer = root.GetComponent<SkinnedMeshRenderer>();
            List<Material> replacement = new List<Material>();
            _childMaterials.Add(meshRenderer.materials);
            foreach (Material m in meshRenderer.materials) replacement.Add(HitFlashMaterial);
            meshRenderer.materials = replacement.ToArray();
        }

        for (int i=0; i < root.childCount; i++) {
            HitFlashMeshes(root.GetChild(i), hitFlashMaterials);
        }
    }

    int ReplaceMeshes(Transform root, int found) {
        int foundHere = 0;
        if (root.GetComponents<MeshRenderer>().Length != 0) {
            MeshRenderer meshRenderer = root.GetComponent<MeshRenderer>();
            if (meshRenderer) {
                meshRenderer.materials = _childMaterials[found];
                foundHere++;
            }
        }
        else if (root.GetComponents<SkinnedMeshRenderer>().Length != 0) {
            SkinnedMeshRenderer meshRenderer = root.GetComponent<SkinnedMeshRenderer>();
            if (meshRenderer) {
                meshRenderer.materials = _childMaterials[found];
                foundHere++;
            }
        }

        for (int i = 0; i < root.childCount; i++) {
            foundHere += ReplaceMeshes(root.GetChild(i), found + foundHere);
        }

        return foundHere;
    }


    void Death()
    {
        // The enemy is dead.
        isDead = true;
        int chance = drop.Next(100);
        if (this.DropChance > chance)
        {
            if (Drops.Length > 0)
            {
                int type = drop.Next(this.Drops.Length - 1);

                if (this.Drops[type])
                {
                    Instantiate(Drops[type], new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                }
            }
        }

        // Used to scatter the bolts
        System.Random rand = new System.Random();
        Vector3 boltOffset;

        for (int i = 0; i < this.BoltDropAmount && this.Bolt; ++i)
        {
            // Move the position of the bolt a little so they're not all overlapping
            boltOffset = new Vector3(
                (float)rand.Next(ScatterMin, ScatterMax) * Scatter,
                (float)rand.Next(ScatterMin, ScatterMax) * Scatter,
                (float)rand.Next(ScatterMin, ScatterMax) * Scatter);

            // Create the bolt
            Instantiate(Bolt, transform.position + boltOffset, transform.rotation);
            SystemLogger.write("Bolt Created");
        }
    }
    
    public void DestroySelf()
    {
        if (DeathEffect) {
            Instantiate(DeathEffect, transform.position, transform.rotation);
        }
        else {
            Instantiate(ParticleHolder.Instance.enemySimpleExplosion, transform.position, Quaternion.identity);
        }
        
        Destroy(this.gameObject);            // delete self
    }

    public void OnDestroy() {
        if (SpawnSystem.Instance) {
            SpawnSystem.Instance.RegisterEnemyDeath();
        }
    }
}