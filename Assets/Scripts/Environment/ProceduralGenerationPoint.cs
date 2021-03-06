﻿using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class ProceduralGenerationPoint {

	public Vector3 position;
	public Vector2 uvPosition;

	public int triangleIndex;
	public float size;
	public string objectName;

	public long gridPosition;
	public long chunkGridPosition;

	public Color c;

	public ProceduralGenerationPoint(Vector3 pos)
	{
		position = pos;
	}
}
