using UnityEngine;
using System.Collections;

public static class Noise
{
	public enum NormalizeMode { Local, Global };

	public static float[,,] GenerateNoiseMap(int mapSize, NoiseSettings settings, Vector3 sampleCentre)
	{
		float[,,] noiseMap = new float[mapSize, mapSize, mapSize];

		System.Random prng = new System.Random(settings.seed);
		Vector3[] octaveOffsets = new Vector3[settings.octaves];

		float maxPossibleHeight = 0;
		float amplitude = 1;
		float frequency = 1;

		for (int i = 0; i < settings.octaves; i++)
		{
			float offsetX = prng.Next(-100000, 100000) + settings.offset.x + sampleCentre.x;
			float offsetY = prng.Next(-100000, 100000) - settings.offset.y - sampleCentre.y;
			float offsetZ = prng.Next(-100000, 100000) - settings.offset.z - sampleCentre.z;
			octaveOffsets[i] = new Vector3(offsetX, offsetY, offsetZ);

			maxPossibleHeight += amplitude;
			amplitude *= settings.persistance;
		}

		float maxLocalNoiseHeight = float.MinValue;
		float minLocalNoiseHeight = float.MaxValue;

		float halfMapSize = mapSize / 2f;


		for (int y = 0; y < mapSize; y++)
		{
			for (int x = 0; x < mapSize; x++)
			{
                for (int z = 0; z < mapSize; z++)
                {
					amplitude = 1;
					frequency = 1;
					float noiseHeight = 0;

					for (int i = 0; i < settings.octaves; i++)
					{
						float sampleX = (x - halfMapSize + octaveOffsets[i].x) / settings.scale * frequency;
						float sampleY = (y - halfMapSize + octaveOffsets[i].y) / settings.scale * frequency;
						float sampleZ = (z - halfMapSize + octaveOffsets[i].z) / settings.scale * frequency;

						float perlinValue = PerlinNoise3D(sampleX, sampleY, sampleZ) * 2 - 1;
						noiseHeight += perlinValue * amplitude;

						amplitude *= settings.persistance;
						frequency *= settings.lacunarity;
					}

					if (noiseHeight > maxLocalNoiseHeight)
					{
						maxLocalNoiseHeight = noiseHeight;
					}
					if (noiseHeight < minLocalNoiseHeight)
					{
						minLocalNoiseHeight = noiseHeight;
					}
					noiseMap[x, y, z] = noiseHeight;

					if (settings.normalizeMode == NormalizeMode.Global)
					{
						float normalizedHeight = (noiseMap[x, y, z] + 1) / (maxPossibleHeight / 0.9f);
						noiseMap[x, y, z] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
					}
				}				
			}
		}

		if (settings.normalizeMode == NormalizeMode.Local)
		{
			for (int y = 0; y < mapSize; y++)
			{
				for (int x = 0; x < mapSize; x++)
				{
                    for (int z = 0; z < mapSize; z++)
					{
						noiseMap[x, y, z] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y, z]);
					}
				}
			}
		}

		return noiseMap;
	}

	public static float PerlinNoise3D(float x, float y, float z)
    {
		float ab = Mathf.PerlinNoise(x, y);
		float bc = Mathf.PerlinNoise(y, z);
		float ac = Mathf.PerlinNoise(x, z);

		float ba = Mathf.PerlinNoise(y, x);
		float cb = Mathf.PerlinNoise(z, y);
		float ca = Mathf.PerlinNoise(z, x);

		float abc = ab + bc + ac + ba + cb + ca;
		return abc / 6f;
	}

}