﻿using UnityEngine;
using System.Collections;

public class CollisionDestroy : MonoBehaviour 
{
	void OnCollisionEnter(Collision collision) 
	{
		
			Destroy(gameObject);
    }
}
