using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapPreviewEditor : Editor
{

	public override void OnInspectorGUI()
	{
		MapGenerator mapPreview = (MapGenerator)target;

		if (DrawDefaultInspector())
		{
			if (mapPreview.autoUpdate)
			{
				mapPreview.DrawBiomeMapTexture();
			}
		}

		if (GUILayout.Button("Generate"))
		{
			mapPreview.DrawBiomeMapTexture();
		}
	}
}