using UnityEngine;
using System.Collections;

public class TerrainPreview : MonoBehaviour 
{
	public enum DrawMode {NoiseMap, Mesh};
	public DrawMode drawMode;
	public bool autoUpdate;

	public Material terrainMaterial;

	public TerrainMapSettings terrainMapSettings;

	public Material mapMaterial;

	public int mapSize;
	public int mapLevelZ;
	public int meshScale = 1;
	public Vector3 sampleCentre;

	Texture2D mapTexture;

	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;

    public void DrawMapInEditor() {
		TerrainMap terrainMap = TerrainMap.Generate(mapSize, terrainMapSettings, sampleCentre);

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
		if (terrainMapSettings != null) {
			terrainMapSettings.OnValuesUpdated -= OnValuesUpdated;
			terrainMapSettings.OnValuesUpdated += OnValuesUpdated;
		}
	}

}
