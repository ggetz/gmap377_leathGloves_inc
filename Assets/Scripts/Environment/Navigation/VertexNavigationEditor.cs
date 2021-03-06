﻿#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

[System.Serializable]
[CustomEditor(typeof(VertexNavigation))]
public class VertexNavigationEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		if(GUILayout.Button("Bake"))
		{
			Debug.Log("beggining baking");

			foreach (Object targ in targets) 
			{
				VertexNavigation vertNav = targ as VertexNavigation;
				vertNav.buildTable();
			}

			Debug.Log("done baking");
		}

		if(GUILayout.Button("Kill Bake"))
		{
			foreach (Object targ in targets) 
			{
				VertexNavigation vertNav = targ as VertexNavigation;
				vertNav.killTable();
			}

			Debug.Log("Killed table");
		}

		if (GUILayout.Button("Modify Vertice Height"))
		{
			Debug.Log("Modifying height of vertices");
			foreach (Object targ in targets) 
			{
				VertexNavigation vertNav = targ as VertexNavigation;
				vertNav.modifyVerticeHeights();
			}
			
			Debug.Log("finished modifying height of vertices");
		}

		if (GUILayout.Button("Set Average Vertex Length"))
		{
			foreach (Object targ in targets) 
			{
				VertexNavigation vertNav = targ as VertexNavigation;
				vertNav.setAverageVerticeLength();
				Debug.Log("average vertex length: " + vertNav.avgVertexlength);
			}
		}
	}

	void OnInspectorUpdate()
	{
		Repaint();
	}
}
#endif