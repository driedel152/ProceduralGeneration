using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class TerrainMapSettings : UpdatableData
{

	public NoiseSettings noiseSettings; 
	public float surfaceLevel;

#if UNITY_EDITOR

	protected override void OnValidate()
	{
		noiseSettings.ValidateValues();
		base.OnValidate();
	}
#endif

}
