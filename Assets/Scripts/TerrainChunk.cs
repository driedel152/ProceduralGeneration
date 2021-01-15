using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk
{
    public static int chunkSize = 16;

    public GameObject meshObject;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private MeshData meshData;

    public Vector3 position;
    private TerrainMapSettings terrainMapSettings;

    bool loaded;

    public TerrainChunk(TerrainMapSettings terrainMapSettings, Material material, Vector3 position)
    {
        loaded = false;

        this.terrainMapSettings = terrainMapSettings;
        this.position = position;

        meshObject = new GameObject("TerrainChunk");
        meshFilter = meshObject.AddComponent<MeshFilter>();
        meshRenderer = meshObject.AddComponent<MeshRenderer>();
        meshCollider = meshObject.AddComponent<MeshCollider>();
        meshCollider.enabled = false;
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
        Mesh mesh = this.meshData.CreateMesh();
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
        loaded = true;
    }

    public void SetVisible(bool visible)
    {
        if (loaded)
        {
            meshObject.SetActive(visible);
        }
    }

    public void SetColliderEnabled(bool enabled)
    {
        meshCollider.enabled = enabled;
    }
}
