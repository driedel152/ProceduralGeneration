using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk
{
    public static int chunkSize = 16;

    public GameObject meshObject;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshData meshData;

    public TerrainChunk(TerrainMapSettings settings, Vector3 position)
    {
        TerrainMap chunk = TerrainMap.Generate(chunkSize, settings, position);
        meshData = MeshGenerator.GenerateTerrainMesh(chunk, position);
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
