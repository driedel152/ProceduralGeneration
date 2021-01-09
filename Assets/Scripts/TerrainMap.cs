using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMap
{
	public readonly float[,,] values;
	public TerrainMapSettings settings;

	private TerrainMap(float[,,] values, TerrainMapSettings settings)
	{
		this.values = values;
		this.settings = settings;
	}

	public static TerrainMap Generate(int mapSize, TerrainMapSettings settings, Vector3 sampleCentre)
	{
		float[,,] values = Noise.GenerateNoiseMap(mapSize, settings.noiseSettings, sampleCentre);

		return new TerrainMap(values, settings);
	}
}
