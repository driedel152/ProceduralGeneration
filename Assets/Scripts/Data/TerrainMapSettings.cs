using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class TerrainMapSettings : UpdatableData
{

	public NoiseSettings noiseSettings;

	public bool useFalloff;

	public float heightMultiplier;

#if UNITY_EDITOR

	protected override void OnValidate()
	{
		noiseSettings.ValidateValues();
		base.OnValidate();
	}
#endif

}
