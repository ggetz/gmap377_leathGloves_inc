﻿using UnityEngine;
using System.Collections;

public class MoveDirectionSelector : MonoBehaviour {

    public float noCloserThan;
    public float asFarAs;

    private MoveTowardsPlayer towardsPlayer;
    private MoveAwayFromPlayer awayFromPlayer;

    void Awake()
    {
        towardsPlayer = this.GetComponent<MoveTowardsPlayer>();
        awayFromPlayer = this.GetComponent<MoveAwayFromPlayer>();
        StartCoroutine(TowardsPlayer());
    }

    void Update() { }

    IEnumerator TowardsPlayer()
    {
        while(Vector3.Angle(-this.transform.up, Player.Instance.transform.position) > noCloserThan)
        {
            yield return new WaitForSeconds(1f);
        }
        towardsPlayer.enabled = false;
        awayFromPlayer.enabled = true;
        StartCoroutine(AwayFromPlayer());
    }

    IEnumerator AwayFromPlayer()
    {
        while(DistanceCalculator.squareEuclidianDistance(Player.Instance.transform.position, this.transform.position) <= asFarAs)
        {
            yield return new WaitForSeconds(2f);
        }
        awayFromPlayer.enabled = false;
        towardsPlayer.enabled = true;
        StartCoroutine(TowardsPlayer());
    }
}
