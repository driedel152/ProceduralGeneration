using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int size; // in chunks

    public TerrainMapSettings terrainSettings;
    public Vector3 sampleCentre;
    public Material terrainMaterial;

    void Start()
    {
        int chunkSize = TerrainChunk.chunkSize - 1;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    Vector3 offset = new Vector3(chunkSize * x, chunkSize * y, chunkSize * z);

                    TerrainChunk chunk = new TerrainChunk(terrainSettings, sampleCentre + offset);
                    chunk.DrawMesh(terrainMaterial);
                    chunk.meshObject.transform.parent = this.gameObject.transform;
                }
            }
        }
    }
}
