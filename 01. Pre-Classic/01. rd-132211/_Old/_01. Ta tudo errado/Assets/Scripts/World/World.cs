using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class World : MonoBehaviour {
    // public static Vector3 worldSizeInBlocks = new Vector3(256, 64, 256);
    public static Vector3 worldSizeInBlocks = new Vector3(32, 64, 32);

    private Vector3 worldSizeInChunks = new Vector3(
        worldSizeInBlocks.x / Chunk.chunkSizeInBlocks.x,
        worldSizeInBlocks.y / Chunk.chunkSizeInBlocks.y,
        worldSizeInBlocks.z / Chunk.chunkSizeInBlocks.z
    );

    private static List<Chunk> chunks = new List<Chunk>();

    private void Awake() {
        //this.SetLayer();
    }

    private void Start() {
        StartCoroutine(this.WorldGen());
    }

    private IEnumerator WorldGen() {
        Vector3 worldSize = new Vector3(
            worldSizeInChunks.x / 2,
            worldSizeInChunks.y,            
            worldSizeInChunks.z / 2
        );

        for(int x = -(int)worldSize.x; x < worldSize.x; x++) {
            for(int z = -(int)worldSize.z; z < worldSize.z; z++) {

                for(int y = 0; y < worldSize.y; y++) {
                    //Chunk chunk = new Chunk(transform, new Vector3(x, y, z));
                    Chunk chunk = new GameObject().AddComponent<Chunk>();
                    chunk.Init(transform, new Vector3(x, y, z));

                    this.GetNeighbor(chunk);
                    
                    chunks.Add(chunk);
                }

                yield return null;
            }
        }

        Player player = new GameObject("Player").AddComponent<Player>();
        player.Init();
    }

    public static Chunk GetChunkBlock(Vector3 pos) {        
        for(int i = 0; i < chunks.Count; i++) {            
            Vector3 chunkPos = chunks[i].transform.position;

            if(
                pos.x < chunkPos.x || pos.x >= chunkPos.x + Chunk.chunkSizeInBlocks.x || 
                pos.y < chunkPos.y || pos.y >= chunkPos.y + Chunk.chunkSizeInBlocks.y || 
                pos.z < chunkPos.z || pos.z >= chunkPos.z + Chunk.chunkSizeInBlocks.z
            ) {
                continue;
            }

            return chunks[i];
        }

        return null;
    }

    public static Chunk GetChunk(Vector3 pos) {
        for(int i = 0; i < chunks.Count; i++) {
            if(chunks[i].transform.position == pos) {
                return chunks[i];
            }
        }

        return null;
    }

    public void GetNeighbor(Chunk chunk) {
        int x = (int)chunk.transform.position.x;
        int y = (int)chunk.transform.position.y;
        int z = (int)chunk.transform.position.z;

        Chunk neighbor = GetChunk(new Vector3(x + 16, y, z));

        if(neighbor != null) {
            chunk.neighbors[0] = neighbor;
            neighbor.neighbors[1] = chunk;
        }
        
        neighbor = GetChunk(new Vector3(x - 16, y, z));

        if(neighbor != null) {
            chunk.neighbors[1] = neighbor;
            neighbor.neighbors[0] = chunk;
        }
        
        neighbor = GetChunk(new Vector3(x, y + 16, z));

        if(neighbor != null) {
            chunk.neighbors[2] = neighbor;
            neighbor.neighbors[3] = chunk;
        }
        
        neighbor = GetChunk(new Vector3(x, y - 16, z));

        if(neighbor != null) {
            chunk.neighbors[3] = neighbor;
            neighbor.neighbors[2] = chunk;
        }
        
        neighbor = GetChunk(new Vector3(x, y, z + 16));

        if(neighbor != null) {
            chunk.neighbors[4] = neighbor;
            neighbor.neighbors[5] = chunk;
        }
        
        neighbor = GetChunk(new Vector3(x, y, z - 16));

        if(neighbor != null) {
            chunk.neighbors[5] = neighbor;
            neighbor.neighbors[4] = chunk;
        }
    }
    
    /*
    private void SetLayer() {
        #if UNITY_EDITOR

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

        #endif
    }
    */
}
