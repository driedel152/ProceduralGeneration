using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class TerrainMapSettings : UpdatableData
{

	public NoiseSettings noiseSettings; 
	[Range(0,1)]
	public float surfaceLevel;
	public float scale;

	public bool applySkyFalloff;
	public int skyFalloffLevel;
	public int skyFalloffLimit;

#if UNITY_EDITOR

	protected override void OnValidate()
	{
		noiseSettings.ValidateValues();
		base.OnValidate();
	}
#endif

}
