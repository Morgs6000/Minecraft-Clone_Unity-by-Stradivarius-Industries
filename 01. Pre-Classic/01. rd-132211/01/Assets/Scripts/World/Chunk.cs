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

    private static Vector3 chunkSizeInBlocks = new Vector3(16, 16, 16);

    private Block[,,] blocks = new Block[
        (int)chunkSizeInBlocks.x,
        (int)chunkSizeInBlocks.y,
        (int)chunkSizeInBlocks.z
    ];

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

    public void BlockMapGen() {
        for(int x = 0; x < chunkSizeInBlocks.x; x++) {
            for(int y = 0; y < chunkSizeInBlocks.y; y++) {
                for(int z = 0; z < chunkSizeInBlocks.z; z++) {                    
                    this.blocks[x, y, z] = Block.GRASS;
                }
            }
        }

        this.ChunkGen();
    }

    private void ChunkGen() {
        for(int x = 0; x < chunkSizeInBlocks.x; x++) {
            for(int y = 0; y < chunkSizeInBlocks.y; y++) {
                for(int z = 0; z < chunkSizeInBlocks.z; z++) {
                    //if(blocks[x, y, z] != null) {
                        this.BlockGen(new Vector3(x, y, z));
                    //}
                }
            }
        }

        this.MeshGen();
    }

    private void BlockGen(Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;

        for(int side = 0; side < 6; side++) {
            if(!HasSolidNeighbor(Block.blockSide[side] + offset)) {
                for(int verts = 0; verts < 4; verts++) {
                    this.vertices.Add(Block.GetVertices()[side, verts] + offset);

                    this.uv.Add(Block.SetUV(Block.GRASS.GetUVCoord())[verts]);
                }
                for(int tris = 0; tris < 6; tris++) {
                    this.triangles.Add(Block.GetTriangles()[tris] + vertexIndex);
                }

                this.vertexIndex += 4;
            }
        }
    }

    private bool HasSolidNeighbor(Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;

        if(
            x < 0 || x > chunkSizeInBlocks.x - 1 ||
            y < 0 || y > chunkSizeInBlocks.y - 1 ||
            z < 0 || z > chunkSizeInBlocks.z - 1
        ) {
            return false;
        }
        if(blocks[x, y, z] == null) {
            return false;
        }

        return true;
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
