using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {
    private Level level;
    
    public int x0;
    public int y0;
    public int z0;

    public int x1;
    public int y1;
    public int z1;

    private static Tesselator t = new Tesselator();

    /**/private MeshFilter meshFilter;
    /**/private MeshRenderer meshRenderer;
    /**/private MeshCollider meshCollider;

    public Chunk(Level level, int x0, int y0, int z0, int x1, int y1, int z1, MeshFilter meshFilter, MeshRenderer meshRenderer, MeshCollider meshCollider) {
        this.level = level;

        this.x0 = x0;
        this.y0 = y0;
        this.z0 = z0;

        this.x1 = x1;
        this.y1 = y1;
        this.z1 = z1;

        this.meshFilter = meshFilter;
        this.meshRenderer = meshRenderer;
        this.meshCollider = meshCollider;

        this.Rebuild();
    }

    private void Rebuild() {
        for(int x = this.x0; x < this.x1; x++) {
            for(int y = this.y0; y < this.y1; y++) {
               for(int z = this.z0; z < this.z1; z++) {
                    if(this.level.IsTile(x, y, z)) {
                        if(y != this.level.height * 2 / 3) {
                            Tile.rock.Render(t, this.level, x, y, z);
                        }
                        else {
                            Tile.grass.Render(t, this.level, x, y, z);
                        }
                    }
                }
            }
        }
        
        t.Flush(this.meshFilter, this.meshRenderer, this.meshCollider);
    }
}
