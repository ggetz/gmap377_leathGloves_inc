﻿using UnityEngine;
using System.Collections;

public class WarpTransport : MonoBehaviour {
    //to do
    //take out shit cody added (week 4)
    public Transform destinationWarpPad;
    public float transportDelaySeconds;

    private ParticleSystem onEffect;

    /// <summary>
    /// Subscribe to activate and deactivate, and initialize the particle effect
    /// </summary>
    void Start()
    {
        ScoreManager.AmountReached += ActivateTeleport;
        ScoreManager.PlayerTeleported += DeactivateTeleport;
        this.onEffect = GetComponentInChildren<ParticleSystem>();
    }

    /// <summary>
    /// When the player stays within the collider, call this to teleport the player
    /// </summary>
    /// <param name="collider"> The object that entered the trigger</param>
    void OnTriggerStay(Collider collider)
    {
        // TODO:  add check for player bolt quantity
        if (collider.gameObject.CompareTag("Player") && ScoreManager.Instance.BoltCount >= ScoreManager.Instance.BoltGoal)
        {
            // Disable player movement while transport is happening
            Player.Instance.GetComponent<ForceMover>().enabled = false;

            // Destroys all currently spawned enemies
            //SpawnSystem.Instance.DestroyAllChildren();

            // Disables spawner while player is warping
            //SpawnSystem.Instance.enabled = false;

            StartCoroutine(TransportAfterTime());
            ScoreManager.Instance.resetBoltCount();
        }
    }

    /// <summary>
    ///     Delays for transportDelaySeconds then moves the player
    ///     and re-enables the ForceMover script
    /// </summary>
    /// <returns></returns>
    IEnumerator TransportAfterTime()
    {
        // Wait for the above specified time
        yield return new WaitForSeconds(transportDelaySeconds);

        // Sets player position to new warp pad location, plus an offset so they are above the warp pad
        Player.Instance.transform.position = destinationWarpPad.position + destinationWarpPad.up;

        // Sets player rotation to the same as the destination warp pad, which will be orientated properly
        Player.Instance.transform.rotation = destinationWarpPad.rotation;

        // Re-enables ForceMover script so player can move again
        // TODO: uncomment this COLANASDFJAS;LDKJA;SKLDFJALSKD;JAS;LKDJASL;KDJ;ALKSDFJLAK;SDF;LKASDFJAL;KSDFJ;LKJ
        Player.Instance.GetComponent<ForceMover>().enabled = true;
        
        // Re-enables SpawnSystem script to create new enemies
        SpawnSystem.Instance.enabled = true;
    }

    /// <summary>
    /// Plays the particle effect
    /// </summary>
    public void ActivateTeleport()
    {
        // Start dat particle effect!
        this.onEffect.Play();
    }

    /// <summary>
    /// Stops the particle effect
    /// </summary>
    public void DeactivateTeleport()
    {
        // Stop dat particle effect
        this.onEffect.Stop();
    }

    /// <summary>
    /// Unsubscribe the events!
    /// </summary>
    void OnApplicationQuit()
    {
        ScoreManager.AmountReached -= ActivateTeleport;
        ScoreManager.PlayerTeleported -= DeactivateTeleport;
    }
}
