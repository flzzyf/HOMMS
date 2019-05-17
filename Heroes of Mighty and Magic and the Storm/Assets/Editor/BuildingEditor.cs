using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Building))]
public class BuildingEditor : Editor
{
	static string prefix = "Building_";

	public override void OnInspectorGUI()
	{
		Building main = (Building)target;

		//显示图标
		if (main.icon != null)
		{
			Texture texture = main.icon.texture;
			GUILayout.Box(texture, EditorStyles.objectFieldThumb,
			 GUILayout.Height(200f));
		}

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Name", LocalizationMgr.instance.GetText(prefix + main.name));
		if (GUILayout.Button("编辑名称"))
		{
			LocalizationWindow.ShowWindow(prefix + main.name);
		}
		EditorGUILayout.EndHorizontal();

		base.OnInspectorGUI();
	}
}
