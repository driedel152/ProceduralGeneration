using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int size; // in chunks

    public TerrainMapSettings terrainSettings;
    public Vector3 sampleCentre;
    public Material terrainMaterial;

    public int horizontalRenderDist; // in chunks
    public int verticalRenderDist; // in chunks

    public static Dictionary<Vector3, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector3, TerrainChunk>();
    List<TerrainChunk> visibleTerrainChunks = new List<TerrainChunk>();

    public Transform viewerTransform;

    private void Update()
    {
        UpdateVisibleTerrainChunks();
    }

    private void UpdateVisibleTerrainChunks()
    {
        foreach (Vector3 chunkCoord in terrainChunkDictionary.Keys)
        {
            terrainChunkDictionary[chunkCoord].SetVisible(false);
        }
        int chunkSize = TerrainChunk.chunkSize - 1;

        int currentChunkCoordX = Mathf.FloorToInt(viewerTransform.position.x / chunkSize);
        int currentChunkCoordY = Mathf.FloorToInt(viewerTransform.position.y / chunkSize);
        int currentChunkCoordZ = Mathf.FloorToInt(viewerTransform.position.z / chunkSize);

        for (int xOffset = -horizontalRenderDist; xOffset <= horizontalRenderDist; xOffset++)
        {
            for (int yOffset = -verticalRenderDist; yOffset <= verticalRenderDist; yOffset++)
            {
                for (int zOffset = -horizontalRenderDist; zOffset < horizontalRenderDist; zOffset++)
                {
                    Vector3 viewedChunkCoord = new Vector3(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset, currentChunkCoordZ + zOffset);
                    if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                    {
                        terrainChunkDictionary[viewedChunkCoord].SetVisible(true);
                    }
                    else
                    {
                        TerrainChunk newChunk = new TerrainChunk(terrainSettings, terrainMaterial, viewedChunkCoord * chunkSize);
                        terrainChunkDictionary.Add(viewedChunkCoord, newChunk);

                        newChunk.meshObject.transform.parent = this.gameObject.transform;
                    }
                }
            }
        }
    }
}
