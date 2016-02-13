﻿using UnityEngine;
using System.Collections;

public abstract class BufferedMovement : AbstractMover 
{
	public float updatePlanBuffer = .5f;

	// Use this for initialization
	void Start () 
	{
		// Start finding plan
		StartCoroutine(this.updatePlan());
	}
	
	/// <summary>
	/// Update the plan in a more buffered approach
	/// </summary>
	/// <returns></returns>
	IEnumerator updatePlan()
	{
		// Always running during gameplay
		while (true)
		{
			this.checkPlan();
			
			yield return new WaitForSeconds(this.updatePlanBuffer);
		}
	}

	public abstract void checkPlan();

	// Update is called once per frame
	void Update()
	{
		// Execute movement
		this.executeCurrentPlan();
	}
}