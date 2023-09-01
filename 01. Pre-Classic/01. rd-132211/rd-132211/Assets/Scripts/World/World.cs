using System.Collections;
using System.Collections.Generic;
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

    private List<Chunk> chunks = new List<Chunk>();

    private void Start() {
        StartCoroutine(WorldGen());
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
                    
                    this.chunks.Add(chunk);
                }

                yield return null;
            }
        }
    }

    public Chunk GetChunk(Vector3 pos) {
        for(int i = 0; i < this.chunks.Count; i++) {
            if(this.chunks[i].transform.position == pos) {
                return this.chunks[i];
            }
        }

        return null;
    }

    public void GetNeighbor(Chunk chunk) {
        int x = (int)chunk.transform.position.x;
        int y = (int)chunk.transform.position.y;
        int z = (int)chunk.transform.position.z;

        Chunk neighbor = this.GetChunk(new Vector3(x + 16, y, z));

        if(neighbor != null) {
            chunk.neighbors[0] = neighbor;
            neighbor.neighbors[1] = chunk;
        }
        
        neighbor = this.GetChunk(new Vector3(x - 16, y, z));

        if(neighbor != null) {
            chunk.neighbors[1] = neighbor;
            neighbor.neighbors[0] = chunk;
        }
        
        neighbor = this.GetChunk(new Vector3(x, y + 16, z));

        if(neighbor != null) {
            chunk.neighbors[2] = neighbor;
            neighbor.neighbors[3] = chunk;
        }
        
        neighbor = this.GetChunk(new Vector3(x, y - 16, z));

        if(neighbor != null) {
            chunk.neighbors[3] = neighbor;
            neighbor.neighbors[2] = chunk;
        }
        
        neighbor = this.GetChunk(new Vector3(x, y, z + 16));

        if(neighbor != null) {
            chunk.neighbors[4] = neighbor;
            neighbor.neighbors[5] = chunk;
        }
        
        neighbor = this.GetChunk(new Vector3(x, y, z - 16));

        if(neighbor != null) {
            chunk.neighbors[5] = neighbor;
            neighbor.neighbors[4] = chunk;
        }
    }
}
