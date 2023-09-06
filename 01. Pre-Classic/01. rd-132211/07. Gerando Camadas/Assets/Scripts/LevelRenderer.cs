using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRenderer : MonoBehaviour {    
    private Level level;

    private Chunk[] chunks;

    private int xChunks;
    private int yChunks;
    private int zChunks;
    
    private void Start() {
        this.level = new Level(256, 64, 256);
        this.Init(this.level);
    }

    private void Update() {
        
    }

    public void Init(Level level) {
        this.level = level;

        this.xChunks = level.width / 16;
        this.yChunks = level.height / 16;
        this.zChunks = level.depth / 16;
               
        this.chunks = new Chunk[this.xChunks * this.yChunks * this.zChunks];

        for(int x = 0; x < this.xChunks; x++) {
            for(int y = 0; y < this.yChunks; y++) {
                for(int z = 0; z < this.zChunks; z++) {
                    // Crie um novo GameObject para cada chunk
                    GameObject newChunk = new GameObject("Chunk: " + x + ", " + y + ", " + z);
                    newChunk.transform.SetParent(this.transform);

                    int x0 = x * 16;
                    int y0 = y * 16;
                    int z0 = z * 16;

                    int x1 = (x + 1) * 16;
                    int y1 = (y + 1) * 16;
                    int z1 = (z + 1) * 16;

                    // Adicione um MeshFilter e um MeshRenderer ao GameObject do chunk
                    MeshFilter meshFilter = newChunk.AddComponent<MeshFilter>();
                    MeshRenderer meshRenderer = newChunk.AddComponent<MeshRenderer>();

                    //Chunk chunk = newChunk.AddComponent<Chunk>();

                    // Crie e associe o Chunk ao objeto de jogo do chunk
                    this.chunks[(x + y * this.xChunks) * this.zChunks + z] = new Chunk(level, x0, y0, z0, x1, y1, z1, meshFilter, meshRenderer);
                }
            }
        }
    }
}
