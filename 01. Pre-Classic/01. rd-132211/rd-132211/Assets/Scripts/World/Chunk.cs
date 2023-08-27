using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
    private Mesh chunkMesh;

    [HideInInspector] public List<Vector3> vertices = new List<Vector3>();
    [HideInInspector] public List<int> triangles = new List<int>();
    [HideInInspector] public List<Vector2> uv = new List<Vector2>();

    [HideInInspector] public int vertexIndex;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    private Material material;

    public static Vector3 chunkSizeInBlocks = new Vector3(16, 256, 16);

    public Block[,,] blocks = new Block[
        (int)chunkSizeInBlocks.x,
        (int)chunkSizeInBlocks.y,
        (int)chunkSizeInBlocks.z
    ];

    private void Awake() {
        this.meshFilter = gameObject.AddComponent<MeshFilter>();
        this.meshRenderer = gameObject.AddComponent<MeshRenderer>();
        this.meshCollider = gameObject.AddComponent<MeshCollider>();

        this.material = Resources.Load<Material>("Materials/Terrain");
    }

    private void Start() {
        this.BlockMap();
    }

    public void setBlock(Vector3 worldPos, Block block) {
        Vector3 localPos = worldPos - transform.position;

        int x = Mathf.FloorToInt(localPos.x);
        int y = Mathf.FloorToInt(localPos.y);
        int z = Mathf.FloorToInt(localPos.z);

        blocks[x, y, z] = block;

        Rebuild();
    }

    public Block getBlock(Vector3 worldPos) {
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

            return default(Block);
        }

        return blocks[x, y, z];
    }

    public void BlockMap() {
        for(int x = 0; x < chunkSizeInBlocks.x; x++) {
            for(int y = 0; y < chunkSizeInBlocks.y; y++) {
                for(int z = 0; z < chunkSizeInBlocks.z; z++) {
                    this.LayerGen(new Vector3(x, y, z));
                }
            }
        }

        this.Rebuild();
    }

    private void LayerGen(Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;

        float _x = x + transform.position.x;
        float _y = y + transform.position.y;
        float _z = z + transform.position.z;

        _x += World.worldSizeInBlocks.x;
        //_y += World.worldSizeInBlocks.y;
        _z += World.worldSizeInBlocks.z;

        if(y < 32) {
            this.blocks[x, y, z] = Block.STONE;
        }
        if(y == 32) {
            this.blocks[x, y, z] = Block.GRASS;
        }
    }

    public void Rebuild() {
        this.Init();

        for(int x = 0; x < chunkSizeInBlocks.x; x++) {
            for(int y = 0; y < chunkSizeInBlocks.y; y++) {
                for(int z = 0; z < chunkSizeInBlocks.z; z++) {
                    if(blocks[x, y, z] != null) {
                        //this.RenderTile(new Vector3(x, y, z));
                        Block.Render(this, new Vector3(x, y, z));
                    }
                }
            }
        }

        this.MeshGen();
    }

    public void Init() {
        this.chunkMesh = new Mesh();
        this.chunkMesh.name = "Chunk";

        this.vertices.Clear();
        this.triangles.Clear();
        this.uv.Clear();

        this.vertexIndex = 0;
    }

    /*
    public void RenderTile(Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;

        for(int side = 0; side < 6; side++) {
            if(!isSolidTile(BlockData.blockSide[side] + offset)) {
                for(int verts = 0; verts < 4; verts++) {
                    this.vertices.Add(BlockData.vertices[side, verts] + offset);
                    this.uv.Add(BlockData.uv(blocks[x, y, z].getUVCoord())[verts]);
                }
                for(int tris = 0; tris < 6; tris++) {
                    this.triangles.Add(BlockData.triangles[tris] + this.vertexIndex);
                }

                this.vertexIndex += 4;
            }
        }
    }
    */

    /*
    public bool isSolidTile(Vector3 offset) {
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
        if(this.blocks[x, y, z] == null) {
            return false;
        }

        return true;
    }
    */

    private void MeshGen() {
        this.chunkMesh.vertices = this.vertices.ToArray();
        this.chunkMesh.triangles = this.triangles.ToArray();
        this.chunkMesh.uv = this.uv.ToArray();

        this.chunkMesh.RecalculateBounds();
        this.chunkMesh.RecalculateNormals();
        this.chunkMesh.RecalculateTangents();
        this.chunkMesh.Optimize();

        this.meshFilter.mesh = this.chunkMesh;
        this.meshRenderer.material = this.material;
        this.meshCollider.sharedMesh = this.chunkMesh;
    }
}
