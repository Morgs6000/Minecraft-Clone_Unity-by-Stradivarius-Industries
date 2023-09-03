using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {
    private GameObject chunkPrefab;
    private Chunk chunk;

    public static Vector3 worldSizeInBlocks = new Vector3(32, 64, 32);

    private Vector3 worldSizeInChunks = new Vector3(
        worldSizeInBlocks.x / Chunk.chunkSizeInBlocks.x,
        worldSizeInBlocks.y / Chunk.chunkSizeInBlocks.y,
        worldSizeInBlocks.z / Chunk.chunkSizeInBlocks.z
    );

    private static List<Chunk> chunks = new List<Chunk>();

    private GameObject playerPrefab;

    private void Awake() {
        this.chunkPrefab = Resources.Load<GameObject>("Prefabs/Chunk");
        this.playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
    }

    private void Start() {
        StartCoroutine(this.WorldGen());
    }

    private void Update() {
        
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
                    this.InstantiateChunk(new Vector3(x, y, z));
                }

                yield return null;
            }
        }

        Instantiate(playerPrefab);
    }

    private void InstantiateChunk(Vector3 chunkPos) {
        int x = (int)chunkPos.x;
        int y = (int)chunkPos.y;
        int z = (int)chunkPos.z;
        
        Vector3 chunkOffset = new Vector3(
            x * Chunk.chunkSizeInBlocks.x,
            y * Chunk.chunkSizeInBlocks.y,
            z * Chunk.chunkSizeInBlocks.z
        );

        Chunk c = GetChunk(new Vector3(
            Mathf.FloorToInt(chunkOffset.x),
            Mathf.FloorToInt(chunkOffset.y),
            Mathf.FloorToInt(chunkOffset.z)
        ));

        if(c == null) {
            GameObject chunkObject = Instantiate(chunkPrefab);
            chunkObject.transform.SetParent(transform);            
            chunkObject.transform.position = chunkOffset;
            chunkObject.name = "Chunk: " + x + ", " + z;

            this.chunk = chunkObject.GetComponent<Chunk>();
            chunks.Add(this.chunk);
        }
    }

    public static Chunk GetChunk(Vector3 pos) {        
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
}
