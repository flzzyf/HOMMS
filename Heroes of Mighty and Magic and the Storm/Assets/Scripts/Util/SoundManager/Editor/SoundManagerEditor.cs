using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		SoundManager sm = (SoundManager)target;
		SerializedProperty trackNum = serializedObject.FindProperty("soundCategoryTrackNum");

		GUILayout.Label("各声音类别的声道数量：");
		for (int i = 0; i < System.Enum.GetValues(typeof(SoundCategory)).Length; i++)
		{
			GUILayout.BeginHorizontal();

			GUILayout.Label(System.Enum.GetName(typeof(SoundCategory), i));
			EditorGUILayout.PropertyField(trackNum.GetArrayElementAtIndex(i), GUIContent.none);

			GUILayout.EndHorizontal();
		}

		serializedObject.ApplyModifiedProperties();
	}
}
