using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
    private Mesh mesh;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uv = new List<Vector2>();

    private int vertexIndex;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    /**/private MeshCollider meshCollider;

    private Material material;

    public static Vector3 chunkSizeInBlocks = new Vector3(16, 16, 16);

    public static string[] blocks = new string[
        (int)chunkSizeInBlocks.x *
        (int)chunkSizeInBlocks.y *
        (int)chunkSizeInBlocks.z
    ];

    private void Awake() {
        this.meshFilter = gameObject.AddComponent<MeshFilter>();
        this.meshRenderer = gameObject.AddComponent<MeshRenderer>();
        /**/this.meshCollider = gameObject.AddComponent<MeshCollider>();

        this.material = Resources.Load<Material>("Materials/Terrain");

        gameObject.layer = LayerMask.NameToLayer("Ground");
    }

    private void Start() {
        this.LevelGen();
    }

    private void Update() {
        
    }

    private string ChunkName() {
        int x = (int)transform.position.x / 16;
        int y = (int)transform.position.y / 16;
        int z = (int)transform.position.z / 16;

        string chunkName = x + ", " + y + ", " + z;

        return chunkName;
    }

    public void SetBlock(Vector3 worldPos, string block) {
        Vector3 localPos = worldPos - transform.position;

        int x = Mathf.FloorToInt(localPos.x);
        int y = Mathf.FloorToInt(localPos.y);
        int z = Mathf.FloorToInt(localPos.z);

        int i = this.ArraySize(x, y, z);

        blocks[i] = block;

        this.ChunkGen();
    }

    public string GetBlock(Vector3 worldPos) {
        Vector3 localPos = worldPos - transform.position;

        int x = Mathf.FloorToInt(localPos.x);
        int y = Mathf.FloorToInt(localPos.y);
        int z = Mathf.FloorToInt(localPos.z);

        if(
            x < 0 || x >= chunkSizeInBlocks.x ||
            y < 0 || y >= chunkSizeInBlocks.y ||
            z < 0 || z >= chunkSizeInBlocks.z
        ) {
            Debug.LogError("Coordinates out of range");

            //return default(Block);
        }

        int i = this.ArraySize(x, y, z);

        return blocks[i];
    }

    private void LevelGen() {
        for(int x = 0; x < chunkSizeInBlocks.x; x++) {
            for(int y = 0; y < chunkSizeInBlocks.y; y++) {
                for(int z = 0; z < chunkSizeInBlocks.z; z++) {
                    LayerGen(new Vector3(x, y, z));
                }
            }
        }
        
        SaveHandler.Load(ChunkName());
        this.ChunkGen();
    }

    private void LayerGen(Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;

        float _x = x + transform.position.x;
        float _y = y + transform.position.y;
        float _z = z + transform.position.z;

        int i = this.ArraySize(x, y, z);

        if(_y < 41) {
            blocks[i] = Block.STONE.blockID;
        }
        else if(_y == 41) {
            blocks[i] = Block.GRASS.blockID;
        }
        else {
            blocks[i] = Block.AIR.blockID;
        }
    }

    public void ChunkGen() {
        mesh = new Mesh();
        mesh.name = "Chunk";

        this.Clear();

        for(int x = 0; x < chunkSizeInBlocks.x; x++) {
            for(int y = 0; y < chunkSizeInBlocks.y; y++) {
                for(int z = 0; z < chunkSizeInBlocks.z; z++) {
                    int i = this.ArraySize(x, y, z);

                    if(blocks[i] != Block.AIR.blockID) {
                        this.BlockGen(new Vector3(x, y, z));
                    }
                }
            }
        }

        SaveHandler.Save(ChunkName());
        this.MeshGen();
    }

    private void Clear() {
        mesh.Clear();

        vertices.Clear();
        triangles.Clear();
        uv.Clear();

        vertexIndex = 0;
    }

    private void BlockGen(Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;

        int i = this.ArraySize(x, y, z);

        for(int side = 0; side < 6; side++) {
            if(!HasSolidNeighbor(Block.blockSide[side] + offset)) {
                for(int verts = 0; verts < 4; verts++) {
                    this.vertices.Add(Block.GetVertices()[side, verts] + offset);

                    this.uv.Add(Block.SetUV(Block.GetBlockID(blocks[i]).GetUVCoord())[verts]);
                }
                for(int tris = 0; tris < 6; tris++) {
                    this.triangles.Add(Block.GetTriangles()[tris] + this.vertexIndex);
                }

                this.vertexIndex += 4;
            }
        }
    }

    private bool HasSolidNeighbor(Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;

        int i = this.ArraySize(x, y, z);

        if(
            x < 0 || x > chunkSizeInBlocks.x -1 ||
            y < 0 || y > chunkSizeInBlocks.y -1 ||
            z < 0 || z > chunkSizeInBlocks.z -1
        ) {
            return false;
        }
        if(blocks[i] == Block.AIR.blockID) {
            return false;
        }

        return true;
    }

    private int ArraySize(int x, int y, int z) {
        return (y * (int)chunkSizeInBlocks.z + z) * (int)chunkSizeInBlocks.x + x;
    }

    private void MeshGen() {
        mesh.vertices = this.vertices.ToArray();
        mesh.triangles = this.triangles.ToArray();
        mesh.uv = this.uv.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.Optimize();

        this.meshFilter.mesh = mesh;
        this.meshRenderer.material = material;
        /**/this.meshCollider.sharedMesh = mesh;
    }
}
