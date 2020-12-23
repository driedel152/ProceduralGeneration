using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MarchingCubeGenerator))]
public class MarchingCubeGeneratorEditor : Editor
{

	public override void OnInspectorGUI()
	{
		MarchingCubeGenerator preview = (MarchingCubeGenerator)target;

		if (DrawDefaultInspector())
		{
			if (preview.autoUpdate)
			{
				preview.DrawTriangles();
			}
		}

		if (GUILayout.Button("Generate"))
		{
			preview.DrawTriangles();
		}
	}
}