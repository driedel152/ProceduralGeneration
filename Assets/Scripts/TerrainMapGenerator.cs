using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TerrainMapGenerator
{
	public static TerrainMap GenerateTerrainMap(int mapSize, TerrainMapSettings settings, Vector3 sampleCentre)
	{
		float[,,] values = Noise.GenerateNoiseMap(mapSize, settings.noiseSettings, sampleCentre);

		return new TerrainMap(values, settings);
	}
}

public class TerrainMap
{
	public readonly float[,,] values;
	public TerrainMapSettings settings;

	public TerrainMap(float[,,] values, TerrainMapSettings settings)
	{
		this.values = values;
		this.settings = settings;
	}
}
