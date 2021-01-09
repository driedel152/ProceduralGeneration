using UnityEngine;
using System.Collections;

public class MapPreview : MonoBehaviour 
{
	public enum DrawMode {NoiseMap, Mesh};
	public DrawMode drawMode;
	public bool autoUpdate;

	public Material terrainMaterial;

	public TerrainMapSettings terrainSettings;

	public Material mapMaterial;

	public int mapSize;
	public int mapLevelZ;
	[Range(0,1)]
	public float surfaceLevel;
	public int meshScale = 1;
	public Vector3 sampleCentre;

	Texture2D mapTexture;

	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;

    private void Start()
    {
		TerrainChunk chunk = new TerrainChunk(terrainSettings, sampleCentre);
		chunk.DrawMesh(terrainMaterial);
	}

    public void DrawMapInEditor() {
		TerrainMap terrainMap = TerrainMapGenerator.GenerateTerrainMap(mapSize, terrainSettings, sampleCentre);

		if (drawMode == DrawMode.NoiseMap) {
			mapTexture = TextureGenerator.TextureFromNoiseMap(terrainMap.values, mapLevelZ);
			mapMaterial.mainTexture = mapTexture;
		} else if (drawMode == DrawMode.Mesh) {
			DrawMesh (MeshGenerator.GenerateTerrainMesh(terrainMap, sampleCentre));
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
		if (terrainSettings != null) {
			terrainSettings.OnValuesUpdated -= OnValuesUpdated;
			terrainSettings.OnValuesUpdated += OnValuesUpdated;
		}
	}

}
