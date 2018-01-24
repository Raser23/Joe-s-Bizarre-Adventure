using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Ladder))]
[CanEditMultipleObjects]
public class LadderEditor:Editor {

	public override void OnInspectorGUI()
	{
        serializedObject.Update();
		DrawDefaultInspector();

        Ladder myScript = (Ladder)target;
		if (GUILayout.Button("Build Ladder"))
		{
            myScript.Build();
		}
	}
}
