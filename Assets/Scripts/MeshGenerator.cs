using UnityEngine;
using System.Collections;

public static class MeshGenerator
{


	public static MeshData GenerateTerrainMesh(float[,,] noiseMap, float surfaceLevel, int size, bool interpolate)
	{
		// assuming it's a cube
		int numVertsPerLine = noiseMap.GetLength(0);
		MeshData meshData = new MeshData(numVertsPerLine);
		int vertexIndex = 0;
		int mapSize = noiseMap.GetLength(0) * size;

        for (int x = 0; x < noiseMap.GetLength(0)-1; x++)
        {
            for (int y = 0; y < noiseMap.GetLength(1)-1; y++)
            {
                for (int z = 0; z < noiseMap.GetLength(2)-1; z++)
                {
					bool[] activeVertices =
					{
						noiseMap[x+1,y,z+1] > surfaceLevel,
						noiseMap[x+1,y,z] > surfaceLevel,
						noiseMap[x,y,z] > surfaceLevel,
						noiseMap[x,y,z+1] > surfaceLevel,
						noiseMap[x+1,y+1,z+1] > surfaceLevel,
						noiseMap[x+1,y+1,z] > surfaceLevel,
						noiseMap[x,y+1,z] > surfaceLevel,
						noiseMap[x,y+1,z+1] > surfaceLevel,
					};

					int iteration = (int)MarchingCubeGenerator.ConvertBoolArrayToByte(activeVertices);
					Vector3[] edges = !interpolate ? MarchingCubeGenerator.GetCubeEdgesAt(x, y, z, size) : MarchingCubeGenerator.GetInterpolatedCubeEdgesAt(x, y, z, size, noiseMap, surfaceLevel);

					for (int i = 0; i < edges.Length; i++)
					{
						Vector3 centeredEdge = edges[i] - new Vector3(mapSize, mapSize, mapSize) / 2f;
						Vector2 percent = new Vector2(x, y) / (numVertsPerLine);
						meshData.AddVertex(centeredEdge, percent, vertexIndex + i);
					}

					for (int triangleVertexIndex = 0; MarchingCubeGenerator.triTable[iteration, triangleVertexIndex] != -1 || triangleVertexIndex > MarchingCubeGenerator.triTable.GetLength(1); triangleVertexIndex += 3)
					{
						meshData.AddTriangle(
							MarchingCubeGenerator.triTable[iteration, triangleVertexIndex] + vertexIndex, 
							MarchingCubeGenerator.triTable[iteration, triangleVertexIndex + 2] + vertexIndex,
							MarchingCubeGenerator.triTable[iteration, triangleVertexIndex + 1] + vertexIndex); // clockwise for some reason
					}
					vertexIndex += 12;
				}
            }
        }

		return meshData;

	}
}

public class MeshData
{
	Vector3[] vertices;
	int[] triangles;
	Vector2[] uvs;

	int triangleIndex;
	int vertexIndex;

	public MeshData(int numVertices)
	{
		int numEdges = (int)Mathf.Pow(numVertices-1, 3) * 12;
		vertices = new Vector3[numEdges];
		uvs = new Vector2[numEdges];
		int maxNumTriangleVerts = (int)Mathf.Pow(numVertices - 1, 3) * 5 * 3;
		triangles = new int[maxNumTriangleVerts];
	}

	public void AddVertex(Vector3 vertexPosition, Vector2 uv, int vertexIndex)
	{
		//Debug.Log($"Adding vertex ({vertexPosition}) at index: {vertexIndex}");
		vertices[vertexIndex] = vertexPosition;
		uvs[vertexIndex] = uv;
	}

	public void AddTriangle(int a, int b, int c)
	{
		//Debug.Log($"Adding triangle ({a} {b} {c}) at index: {triangleIndex}");
		triangles[triangleIndex] = a;
		triangles[triangleIndex + 1] = b;
		triangles[triangleIndex + 2] = c;
		triangleIndex += 3;
	}

	public Mesh CreateMesh()
	{
		Mesh mesh = new Mesh();
		mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		return mesh;
	}

}