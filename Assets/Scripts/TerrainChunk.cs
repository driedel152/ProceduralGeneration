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

    public Vector3 position;
    private TerrainMapSettings terrainMapSettings;

    public TerrainChunk(TerrainMapSettings terrainMapSettings, Material material, Vector3 position)
    {
        this.terrainMapSettings = terrainMapSettings;
        this.position = position;

        meshObject = new GameObject("TerrainChunk");
        meshFilter = meshObject.AddComponent<MeshFilter>();
        meshRenderer = meshObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;

        RequestTerrainMap();
    }

    private void RequestTerrainMap()
    {
        ThreadedDataRequester.RequestData(() => TerrainMap.Generate(chunkSize, terrainMapSettings, position), OnTerrainMapReceived);
    }

    private void OnTerrainMapReceived(object terrainMap)
    {
        ThreadedDataRequester.RequestData(() => MeshGenerator.GenerateTerrainMesh((TerrainMap)terrainMap, position), OnMeshDataReceived);
    }

    private void OnMeshDataReceived(object meshData)
    {
        this.meshData = (MeshData)meshData;
        meshFilter.sharedMesh = this.meshData.CreateMesh();
        meshFilter.gameObject.SetActive(true);
    }

    public void SetVisible(bool visible)
    {
        meshObject.SetActive(visible);
    }
}
