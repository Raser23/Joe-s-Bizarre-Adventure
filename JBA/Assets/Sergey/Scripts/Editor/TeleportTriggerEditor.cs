using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TeleportTrigger))]
[CanEditMultipleObjects]
public class TeleportTriggerEditor : Editor {

    SerializedProperty currentType,mask;

	void OnEnable()
	{
		currentType = serializedObject.FindProperty("currentType");
		mask = serializedObject.FindProperty("mask");

	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		
        EditorGUILayout.PropertyField(currentType);

        if(currentType.enumValueIndex == 0){//Only player
            EditorGUILayout.PrefixLabel("HUI");
        }
		if (currentType.enumValueIndex == 1)//LayerMask
		{
            EditorGUILayout.PropertyField(mask);
		}
		if (currentType.enumValueIndex == 2)//tags
		{
			EditorGUILayout.PrefixLabel("Пока не равботает");
		}

		//DrawDefaultInspector();
		serializedObject.ApplyModifiedProperties();
		
	}
}
