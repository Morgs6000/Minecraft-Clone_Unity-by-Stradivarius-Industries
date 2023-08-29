using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uv = new List<Vector2>();

    private int vertexIndex;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private Material material;

    private Vector3 chunkSizeInBlocks = new Vector3(16, 16, 16);

    public Chunk(Transform world) {
        GameObject newChunk = new GameObject();

        int x = Mathf.FloorToInt(newChunk.transform.position.x);
        int y = Mathf.FloorToInt(newChunk.transform.position.y);
        int z = Mathf.FloorToInt(newChunk.transform.position.z);

        newChunk.name = "Chunk: " + x + ", " + y + ", " + z;
        newChunk.transform.parent = world;

        this.meshFilter = newChunk.AddComponent<MeshFilter>();
        this.meshRenderer = newChunk.AddComponent<MeshRenderer>();

        this.material = Resources.Load<Material>("Materials/Terrain");

        this.ChunkGen();
    }

    private void ChunkGen() {
        for(int x = 0; x < this.chunkSizeInBlocks.x; x++) {
            for(int y = 0; y < this.chunkSizeInBlocks.y; y++) {
                for(int z = 0; z < chunkSizeInBlocks.z; z++) {
                    this.BlockGen(new Vector3(x, y, z));
                }
            }
        }

        this.MeshGen();
    }

    private void BlockGen(Vector3 offset) {
        for(int side = 0; side < 6; side++) {
            for(int verts = 0; verts < 4; verts++) {
                this.vertices.Add(Block.vertices[side, verts] + offset);

                this.uv.Add(Block.uv(new Vector2(0, 0))[verts]);
            }
            for(int tris = 0; tris < 6; tris++) {
                this.triangles.Add(Block.triangles[tris] + vertexIndex);
            }

            this.vertexIndex += 4;
        }
    }

    private void MeshGen() {
        Mesh mesh = new Mesh();
        mesh.name = "Chunk";

        mesh.vertices = this.vertices.ToArray();
        mesh.triangles = this.triangles.ToArray();
        mesh.uv = this.uv.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.Optimize();

        this.meshFilter.mesh = mesh;
        this.meshRenderer.material = material;
    }
}
