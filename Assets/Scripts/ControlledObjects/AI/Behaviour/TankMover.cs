﻿using UnityEngine;
using System.Collections;
using System;

public class TankMover : BufferedMovement {

    private VertexNavigation planetVertexNavigation;

    public bool playerInRange;
    private bool playerRangeChanged = false;

    void Start()
    {
        base.init();

        base.targetLocation = Player.Instance.getClosestVertice();
		base.setMovementScript(this.GetComponent<AStar>());
		base.moveTowardsPlayerAtEndOfPath = false;

		this.getNewPlan(Player.Instance.transform.position);
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            playerRangeChanged = true;
        }
    }

    void OnTriggerExit(Collider obj)
    {
        if (obj.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            playerRangeChanged = true;
        }
    }

    public override void checkPlan()
    {
        if (this.shouldUpdatePlan())
        {
            playerRangeChanged = false;
            base.resetTargetIndex();

            // TODO: Use x vertices away functionality in updated A*
            if (this.playerInRange)
            {
                // set target to be above the planet (TODO: remove magic number)
				this.targetLocation = this.transform.position + this.transform.forward * -1 * this.aiMovement.GetPlanetVertexNavigation().Radius;

                // Place target onto planet by using raycast from the point away from planet that we calculated with cross product
				RaycastHit[] hits = Physics.RaycastAll(this.targetLocation, this.aiMovement.GetPlanetVertexNavigation().transform.position - this.targetLocation, this.aiMovement.GetPlanetVertexNavigation().Radius);

                for (int i = 0; i < hits.Length; ++i)
                {
                    // check if collision was the planet
                    if (hits[i].collider.CompareTag("Planet"))
                    {
                        // Set target location to collision point on planet
                        this.targetLocation = hits[i].point;
                        break;
                    }
                }

                base.setTarget(this.targetLocation);
            }
            else
            {
                base.setTarget(Player.Instance.transform.position);
            }

            this.getNewPlan(base.targetLocation);
        }
    }

	/// <summary>
	/// Move Towards the given target
	/// </summary>
	/// <param name="targ"></param>
    public override void move(Vector3 targ)
    {
        if (playerInRange)
        {
            // Set rotation
            Quaternion destRotation = Quaternion.LookRotation(Player.Instance.transform.position - transform.position, transform.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, destRotation, rotationMaxDegrees * Time.deltaTime);

            // Move backwards
            this.transform.position = Vector3.MoveTowards(this.transform.position, targ, this.moveSpeed * Time.deltaTime);
        }
        else
        {
            // Default movement
            base.move(targ);
        }
    }

    public override bool shouldUpdatePlan()
    {
        return (this.plan == null || DistanceCalculator.squareEuclidianDistance(base.targetLocation, Player.Instance.transform.position) >= base.minMoveDistance || playerRangeChanged);
    }
}