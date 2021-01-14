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

        if (settings.applySkyFalloff)
        {
            for (int x = 0; x < mapSize; x++)
            {
                for (int y = 0; y < mapSize; y++)
                {
                    for (int z = 0; z < mapSize; z++)
                    {
                        if(y + sampleCentre.y > settings.skyFalloffLevel)
                        {
                            values[x, y, z] -= Mathf.InverseLerp(settings.skyFalloffLevel, settings.skyFalloffLimit, y + sampleCentre.y);
                        }
                    }
                }
            }
        }

		return new TerrainMap(values, settings);
	}
}
