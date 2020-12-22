using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public bool autoUpdate;

    public NoiseSettings noiseSettings;

    public Material mapMaterial;

    public int mapSize;
	public int mapLevelZ;

    Texture2D mapTexture;

    public void DrawBiomeMapTexture()
    {
		float[,,] noiseMap = Noise.GenerateNoiseMap(mapSize, noiseSettings, Vector2.zero);
		mapTexture = TextureGenerator.TextureFromNoiseMap(noiseMap, mapLevelZ);
		mapMaterial.mainTexture = mapTexture;
		
    }

	void OnValuesUpdated()
	{
		if (!Application.isPlaying)
		{
			DrawBiomeMapTexture();
		}
	}

	void OnValidate()
	{
		if (noiseSettings != null)
		{
			noiseSettings.OnValuesUpdated -= OnValuesUpdated;
			noiseSettings.OnValuesUpdated += OnValuesUpdated;
		}

	}

}
