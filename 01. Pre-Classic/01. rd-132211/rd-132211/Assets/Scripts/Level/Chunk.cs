using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {  
    public Level level;

    public int x0;
    public int y0;
    public int z0;

    public int x1;
    public int y1;
    public int z1;

    private bool dirty = true;

    private static Tesselator t = new Tesselator();

    public static int rebuiltThisFrame = 0;
    public static int updates = 0;

    public Chunk(Level level, int x0, int y0, int z0, int x1, int y1, int z1) {
        this.level = level;

        this.x0 = x0;
        this.y0 = y0;
        this.z0 = z0;

        this.x1 = x1;
        this.y1 = y1;
        this.z1 = z1;
    }

    private void Rebuild(int layer) {
        if(rebuiltThisFrame != 2) {
            this.dirty = false;

            updates++;
            rebuiltThisFrame++;

            int tiles = 0;

            for(int x = this.x0; x < this.x1; x++) {
                for(int y = this.y0; y < this.y1; y++) {
                    for(int z = this.z0; z < this.z1; z++) {
                        bool tex = y != this.level.depth * 2 / 3;

                        tiles++;

                        if(!tex) {
                            Tile.rock.Render(t, this.level, layer, x, y, z);
                        }
                        else {
                            Tile.grass.Render(t, this.level, layer, x, y, z);
                        }
                    }
                }
            }

            t.Flush();
        }
    }

    public void Render(int layer) {
        if(this.dirty) {
            this.Rebuild(0);
            this.Rebuild(1);
        }
    }

    public void SetDirt() {
        this.dirty = true;
    }
}
