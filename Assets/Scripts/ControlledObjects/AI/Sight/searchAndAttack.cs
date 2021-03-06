﻿using UnityEngine;
using System.Collections;

public class searchAndAttack : MonoBehaviour 
{
	public AISight sight;
	public Transform attackSpawnPoint;
	public GameObject weapon;
	public float fireRate;
	public bool bossAttack;
    public bool turretScriptOnlyOne;

	[Tooltip("check if you want the weapon to be attached to the spawnpoint")]
	public bool AttachToSpawnPointOnFire = false;

	// timer for shooting
	private float timer = 0f;

	// player found
	private bool playerFound = false;

    private Animator bossAnimator;

    void Start()
    {
        if (bossAttack)
        {
            bossAnimator = this.GetComponentInParent<Animator>();
        }
    }

	public bool PlayerFound 
	{
		get
		{
			return this.playerFound;
		}
		set 
		{ 
			// pass
		}
	}

	// Update is called once per frame
	void Update () 
	{
		// Search
		Transform targ = sight.LookForPlayer();

		// add to timer
		timer += Time.deltaTime;

		// Check targ
		if(targ != null)
		{
            if (!this.bossAttack)
            {
                // do new rotation to rotate around the Vector3.up
                //transform.Rotate(Vector3.up, Vector3.Angle(targ.position, this.transform.position));
                //float angle = Vector3.Angle(this.transform.forward, Player.Instance.transform.position - this.transform.position);
                //Debug.Log("angle: " + angle);

                //transform.Rotate(Vector3.up, angle);
                Vector3 targetPosition = targ.position;
                Vector3 newRotation = Quaternion.LookRotation(targetPosition - this.transform.position, this.transform.up).eulerAngles;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(newRotation), Time.deltaTime);

            }
            // Mark the player as found
			this.playerFound = true;

            // Check if it's time to fire again
			if(timer > fireRate)
			{
				// instantiate bullet
				GameObject bullet = Instantiate(this.weapon, this.attackSpawnPoint.position, this.transform.rotation) as GameObject; 

				// Check if it should be attached to player
				if(this.AttachToSpawnPointOnFire)
				{
					bullet.transform.parent = this.attackSpawnPoint;
				}
				else
				{
					// add target to bullet
					bullet.GetComponent<FireForward>().target = targ.position;
				}

                if(bossAttack && turretScriptOnlyOne)
                {
                    bossAnimator.SetTrigger("FireTurret");   
                }

				// reset timer
				timer = 0f;
			}
		}
	}
}
