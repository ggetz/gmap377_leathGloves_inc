﻿using UnityEngine;
using System.Collections;

public class BoltCollection : MonoBehaviour
{
    // How much it increases the bolt count by
    public float BoltIncreaseAmount = 5f;
    System.Random rand;

    public float
        Range = 5,
        MoveSpeed = 2f,
        ScatterForceRange = 5f;

    public LayerMask TargetLayer;

    void Start()
    {
        rand = new System.Random();
        GetComponent<Rigidbody>().AddForce(new Vector3(rand.Next(-(int)ScatterForceRange, (int)ScatterForceRange), rand.Next(-(int)ScatterForceRange, (int)ScatterForceRange), rand.Next(-(int)ScatterForceRange, (int)ScatterForceRange)));
    }

    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, this.Range, TargetLayer);

        for (int i = 0; i < hitColliders.Length; ++i)
        {
            moveTowardsPlayer(hitColliders[0].gameObject.transform);
        }
    }

    public void moveTowardsPlayer(Transform player)
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, MoveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// When the bolt is collected, add the specified amount to the bolt count.
    /// </summary>
    /// <param name="player"></param>
    void OnCollisionEnter(Collision player)
    {
        // If the object is the player
        if(player.gameObject.tag == "Player")
        {
            // Increase the bolt count
            ScoreManager.Instance.collectBolt(this.BoltIncreaseAmount);
            Destroy(this.gameObject);
        }
    }
}
