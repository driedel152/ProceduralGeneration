using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TerrainMapGenerator
{
	public static TerrainMap GenerateTerrainMap(int mapSize, TerrainMapSettings settings, Vector3 sampleCentre)
	{
		float[,,] values = Noise.GenerateNoiseMap(mapSize, settings.noiseSettings, sampleCentre);

		AnimationCurve heightCurve_threadsafe = new AnimationCurve(settings.heightCurve.keys);

		float minValue = float.MaxValue;
		float maxValue = float.MinValue;

		for (int i = 0; i < mapSize; i++)
		{
			for (int j = 0; j < mapSize; j++)
			{
				values[i, 0, j] *= heightCurve_threadsafe.Evaluate(values[i, 0, j]) * settings.heightMultiplier;

				if (values[i, 0, j] > maxValue)
				{
					maxValue = values[i, 0, j];
				}
				if (values[i, 0, j] < minValue)
				{
					minValue = values[i, 0, j];
				}
			}
		}

		return new TerrainMap(values, minValue, maxValue);
	}
}

public struct TerrainMap
{
	public readonly float[,,] values;
	public readonly float minValue;
	public readonly float maxValue;

	public TerrainMap(float[,,] values, float minValue, float maxValue)
	{
		this.values = values;
		this.minValue = minValue;
		this.maxValue = maxValue;
	}
}
