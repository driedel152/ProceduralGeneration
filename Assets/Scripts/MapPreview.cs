using UnityEngine;
using System.Collections;

public class MapPreview : MonoBehaviour 
{
	public enum DrawMode {NoiseMap, Mesh};
	public DrawMode drawMode;
	public bool autoUpdate;

	public Material terrainMaterial;

	public NoiseSettings noiseSettings;

	public Material mapMaterial;

	public int mapSize;
	public int mapLevelZ;
	[Range(0,1)]
	public float surfaceLevel;
	public int meshScale = 1;

	Texture2D mapTexture;

	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;



	public void DrawMapInEditor() {
		float[,,] noiseMap = Noise.GenerateNoiseMap(mapSize, noiseSettings, Vector3.zero);

		if (drawMode == DrawMode.NoiseMap) {
			mapTexture = TextureGenerator.TextureFromNoiseMap(noiseMap, mapLevelZ);
			mapMaterial.mainTexture = mapTexture;
		} else if (drawMode == DrawMode.Mesh) {
			DrawMesh (MeshGenerator.GenerateTerrainMesh(noiseMap, surfaceLevel, meshScale));
		} 
	}

	public void DrawMesh(MeshData meshData) {
		meshFilter.sharedMesh = meshData.CreateMesh ();

		meshFilter.gameObject.SetActive (true);
	}



	void OnValuesUpdated() {
		if (!Application.isPlaying) {
			DrawMapInEditor ();
		}
	}

	void OnValidate() 
	{
		if (noiseSettings != null) {
			noiseSettings.OnValuesUpdated -= OnValuesUpdated;
			noiseSettings.OnValuesUpdated += OnValuesUpdated;
		}
	}

}
