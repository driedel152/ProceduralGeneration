using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk
{
    public static int chunkSize = 16;

    GameObject meshObject;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    MeshData meshData;

    public TerrainChunk(TerrainMapSettings settings, Vector3 sampleCentre)
    {
        TerrainMap chunk = TerrainMap.Generate(chunkSize, settings, sampleCentre);
        meshData = MeshGenerator.GenerateTerrainMesh(chunk, sampleCentre);
    }

    public void DrawMesh(Material material)
    {
        meshObject = new GameObject("TerrainChunk");
        meshFilter = meshObject.AddComponent<MeshFilter>();
        meshRenderer = meshObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;

        meshFilter.sharedMesh = meshData.CreateMesh();
        meshFilter.gameObject.SetActive(true);
    }
}
