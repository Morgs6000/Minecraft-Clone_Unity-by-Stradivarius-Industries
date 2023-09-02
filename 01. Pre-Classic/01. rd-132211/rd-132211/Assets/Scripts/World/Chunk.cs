using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Chunk : MonoBehaviour {
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uv = new List<Vector2>();

    private int vertexIndex;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    /**/private MeshCollider meshCollider;

    private Material material;

    public static Vector3 chunkSizeInBlocks = new Vector3(16, 16, 16);

    public static string[,,] blocks = new string[
        (int)chunkSizeInBlocks.x,
        (int)chunkSizeInBlocks.y,
        (int)chunkSizeInBlocks.z
    ];

    public Chunk[] neighbors = new Chunk[6];

    public void Init(Transform world, Vector3 offset) {
        //GameObject newChunk = new GameObject();

        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;

        name = "Chunk: " + x + ", " + y + ", " + z;
        transform.parent = world;
        
        Vector3 chunkOffset = new Vector3(
            x * chunkSizeInBlocks.x,
            y * chunkSizeInBlocks.y,
            z * chunkSizeInBlocks.z
        );

        transform.position = chunkOffset;

        //Chunk chunk = gameObject.AddComponent<Chunk>();
        this.meshFilter = gameObject.AddComponent<MeshFilter>();
        this.meshRenderer = gameObject.AddComponent<MeshRenderer>();
        /**/this.meshCollider = gameObject.AddComponent<MeshCollider>();

        this.material = Resources.Load<Material>("Materials/Terrain");

        this.BlockMapGen();

        //this.SetLayer();
        gameObject.layer = LayerMask.NameToLayer("Ground");;
    }

    /*
    private void SetLayer() {
        // Nome da nova camada
        string novaLayerNome = "Ground";
        
        int novaLayer = LayerMask.NameToLayer(novaLayerNome);
        
        if(novaLayer == -1) {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty layersProp = tagManager.FindProperty("layers");
            
            for (int i = 8; i < 32; i++) // Comece a procurar a partir do índice 8 (índice 0-7 são camadas padrão)
            {
                SerializedProperty layerProp = layersProp.GetArrayElementAtIndex(i);
                if (layerProp.stringValue == "")
                {
                    layerProp.stringValue = novaLayerNome;
                    tagManager.ApplyModifiedProperties();
                    //Debug.Log($"Nova camada '{novaLayerNome}' criada na camada {i}.");
                    return;
                }
            }
        }

        gameObject.layer = novaLayer;
    }
    */

    public void SetBlock(Vector3 worldPos, string block) {
        Vector3 localPos = worldPos - transform.position;

        int x = Mathf.FloorToInt(localPos.x);
        int y = Mathf.FloorToInt(localPos.y);
        int z = Mathf.FloorToInt(localPos.z);

        blocks[x, y, z] = block;

        ChunkGen();
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

        return blocks[x, y, z];
    }

    public void BlockMapGen() {
        for(int x = 0; x < chunkSizeInBlocks.x; x++) {
            for(int y = 0; y < chunkSizeInBlocks.y; y++) {
                for(int z = 0; z < chunkSizeInBlocks.z; z++) {                    
                    LayerGen(new Vector3(x, y, z));
                }
            }
        }

        this.ChunkGen();
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

        if(_y < 41) {
            blocks[x, y, z] = Block.STONE.blockID;
        }
        else if(_y == 41) {
            blocks[x, y, z] = Block.GRASS.blockID;
        }
        else {
            blocks[x, y, z] = Block.AIR.blockID;
        }
    }

    private void ChunkGen() {
        this.Clear();

        for(int x = 0; x < chunkSizeInBlocks.x; x++) {
            for(int y = 0; y < chunkSizeInBlocks.y; y++) {
                for(int z = 0; z < chunkSizeInBlocks.z; z++) {
                    if(blocks[x, y, z] != Block.AIR.blockID) {
                        this.BlockGen(new Vector3(x, y, z));
                    }
                }
            }
        }

        this.MeshGen();
    }

    private void Clear() {
        vertices.Clear();
        triangles.Clear();
        uv.Clear();

        vertexIndex = 0;
    }

    private void BlockGen(Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;

        for(int side = 0; side < 6; side++) {
            if(!HasSolidNeighbor(Block.blockSide[side] + offset)) {
                for(int verts = 0; verts < 4; verts++) {
                    this.vertices.Add(Block.GetVertices()[side, verts] + offset);

                    this.uv.Add(Block.SetUV(Block.GetBlockID(blocks[x, y, z]).GetUVCoord())[verts]);
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

        if(x > chunkSizeInBlocks.x - 1) {
            if(neighbors[0]) {
                return neighbors[0].HasSolidNeighbor(new Vector3(x + chunkSizeInBlocks.x, y, z));
            }
            else {
                return false;
            }
        }
        if(x < 0) {
            if(neighbors[1]) {
                return neighbors[1].HasSolidNeighbor(new Vector3(x - chunkSizeInBlocks.x, y, z));
            }
            else {
                return false;
            }
        }
        if(y > chunkSizeInBlocks.y - 1) {
            if(neighbors[2]) {
                return neighbors[2].HasSolidNeighbor(new Vector3(x, y + chunkSizeInBlocks.y, z));
            }
            else {
                return false;
            }
        }
        if(y < 0) {
            if(neighbors[3]) {
                return neighbors[3].HasSolidNeighbor(new Vector3(x, y - chunkSizeInBlocks.y, z));
            }
            else {
                return false;
            }
        }
        if(z > chunkSizeInBlocks.z - 1) {
            if(neighbors[4]) {
                return neighbors[4].HasSolidNeighbor(new Vector3(x, y, z + chunkSizeInBlocks.z));
            }
            else {
                return false;
            }
        }
        if(z < 0) {
            if(neighbors[5]) {
                return neighbors[5].HasSolidNeighbor(new Vector3(x, y, z - chunkSizeInBlocks.z));
            }
            else {
                return false;
            }
        }

        return !Block.GetBlockID(blocks[x, y, z]).GetTransparent();
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
        /**/this.meshCollider.sharedMesh = mesh;
    }
}
