using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int size; // in chunks

    public TerrainMapSettings terrainSettings;
    public Vector3 sampleCentre;
    public Material terrainMaterial;

    public int renderDist; // in chunks

    public static Dictionary<Vector3, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector3, TerrainChunk>();
    List<TerrainChunk> visibleTerrainChunks = new List<TerrainChunk>();

    public Transform viewerTransform;

    private void Update()
    {
        UpdateVisibleTerrainChunks();
    }

    private void UpdateVisibleTerrainChunks()
    {
        int chunkSize = TerrainChunk.chunkSize - 1;

        int currentChunkCoordX = Mathf.FloorToInt(viewerTransform.position.x / chunkSize);
        int currentChunkCoordY = Mathf.FloorToInt(viewerTransform.position.y / chunkSize);
        int currentChunkCoordZ = Mathf.FloorToInt(viewerTransform.position.z / chunkSize);

        for (int yOffset = -renderDist; yOffset <= renderDist; yOffset++)
        {
            for (int xOffset = -renderDist; xOffset <= renderDist; xOffset++)
            {
                for (int zOffset = -renderDist; zOffset < renderDist; zOffset++)
                {
                    Vector3 viewedChunkCoord = new Vector3(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset, currentChunkCoordZ + zOffset);
                    if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                    {
                        terrainChunkDictionary[viewedChunkCoord].SetVisible(true);
                    }
                    else
                    {
                        TerrainChunk newChunk = new TerrainChunk(terrainSettings, viewedChunkCoord * chunkSize);
                        terrainChunkDictionary.Add(viewedChunkCoord, newChunk);

                        newChunk.DrawMesh(terrainMaterial);
                        newChunk.meshObject.transform.parent = this.gameObject.transform;
                        Debug.Log("Created new chunk at " + viewedChunkCoord);
                    }
                }
            }
        }
    }
}
