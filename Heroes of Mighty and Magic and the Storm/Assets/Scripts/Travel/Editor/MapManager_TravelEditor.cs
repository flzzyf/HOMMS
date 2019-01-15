using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapManager_Travel))]
public class MapManager_TravelEditor : Editor
{
	MapManager_Travel main;

	public override void OnInspectorGUI()
	{
		main = (MapManager_Travel)target;

		base.OnInspectorGUI();

		if (GUILayout.Button("Generate Map"))
		{
			main.GenerateMap();
		}

		serializedObject.ApplyModifiedProperties();
	}
}
