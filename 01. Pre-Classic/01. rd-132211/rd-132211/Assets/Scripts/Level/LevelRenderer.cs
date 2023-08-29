using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRenderer : LevelListener {
    private static int CHUNK_SIZE = 16;

    private Level level;

    public Chunk[] chunks;

    private int xChunks;
    private int yChunks;
    private int zChunks;

    private Tesselator t = new Tesselator();

    public LevelRenderer(Level level) {
        this.level = level;
        level.AddListerner(this);

        this.xChunks = level.width / 16;
        this.yChunks = level.depth / 16;
        this.zChunks = level.height / 16;

        this.chunks = new Chunk[this.xChunks * this.yChunks * this.zChunks];

        for(int x = 0; x < this.xChunks; x++) {
            for(int y = 0; y < this.yChunks; y++) {
                for(int z = 0; z < this.zChunks; z++) {
                    int x0 = x * 16;
                    int y0 = y * 16;
                    int z0 = z * 16;

                    int x1 = (x + 1) * 16;
                    int y1 = (y + 1) * 16;
                    int z1 = (z + 1) * 16;

                    if(x1 > level.width) {
                        x1 = level.width;
                    }
                    if(y1 > level.depth) {
                        y1 = level.depth;
                    }
                    if(z1 > level.height) {
                        z1 = level.height;
                    }
                    
                    this.chunks[(x + y * this.xChunks) * this.zChunks + z] = new Chunk(level, x0, y0, z0, x1, y1, z1);
                }
            }
        }
    }

    public void Render(Player player, int layer) {
        Chunk.rebuiltThisFrame = 0;

        for(int i = 0; i < this.chunks.Length; i++) {
            
        }
    }
}
