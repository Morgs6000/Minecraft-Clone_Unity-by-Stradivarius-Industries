using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
    // public AABB aabb;
    // public final Level level;
    public Level level;
    // public final int x0;
    public int x0;
    // public final int y0;
    public int y0;
    // public final int z0;
    public int z0;
    // public final int x1;
    public int x1;
    // public final int y1;
    public int y1;
    // public final int z1;
    public int z1;
    // private boolean dirty = true;
    // private int lists = -1;
    // private static int texture = Textures.loadTexture("/terrain.png", 9728);
    private static Tesselator t = new Tesselator();
    // public static int rebuiltThisFrame = 0;
    // public static int updates = 0;

    /**/private void Start() {
        this.level = new Level(16, 16, 16);

        int x = 0;
        int y = 0;
        int z = 0;

        x0 = x * 16;
        y0 = y * 16;
        z0 = z * 16;
        x1 = (x + 1) * 16;
        y1 = (y + 1) * 16;
        z1 = (z + 1) * 16;

        this.rebuild();
    }

    // public Chunk(Level level, int x0, int y0, int z0, int x1, int y1, int z1) {
    public Chunk(int x0, int y0, int z0, int x1, int y1, int z1) {
    //     this.level = level;
        this.x0 = x0;
        this.y0 = y0;
        this.z0 = z0;
        this.x1 = x1;
        this.y1 = y1;
        this.z1 = z1;
    //     this.aabb = new AABB((float)x0, (float)y0, (float)z0, (float)x1, (float)y1, (float)z1);
    //     this.lists = GL11.glGenLists(2);

        /**/this.rebuild();
    }

    // private void rebuild(int layer) {
    private void rebuild() {
    //     if (rebuiltThisFrame != 2) {
    //         this.dirty = false;
    //         ++updates;
    //         ++rebuiltThisFrame;
    //         GL11.glNewList(this.lists + layer, 4864);
    //         GL11.glEnable(3553);
    //         GL11.glBindTexture(3553, texture);
    //         t.init();
    //         int tiles = 0;

            for(int x = this.x0; x < this.x1; ++x) {
                for(int y = this.y0; y < this.y1; ++y) {
                    for(int z = this.z0; z < this.z1; ++z) {
                        if (this.level.isTile(x, y, z)) {
    //                         int tex = y != this.level.depth * 2 / 3;
                            bool tex = y != this.level.depth * 2 / 3;
    //                         ++tiles;
                            if (!tex) {
    //                             Tile.rock.render(t, this.level, layer, x, y, z);
                                Tile.rock.render(t, this.level, x, y, z);
                            } else {
    //                             Tile.grass.render(t, this.level, layer, x, y, z);
                                Tile.grass.render(t, this.level, x, y, z);
                            }
                        }
                    }
                }
            }

    //         t.flush();
            t.flush(gameObject.AddComponent<MeshFilter>(), gameObject.AddComponent<MeshRenderer>());
    //         GL11.glDisable(3553);
    //         GL11.glEndList();
    //     }
    }

    // public void render(int layer) {
    //     if (this.dirty) {
    //         this.rebuild(0);
    //         this.rebuild(1);
    //     }

    //     GL11.glCallList(this.lists + layer);
    // }

    // public void setDirty() {
    //     this.dirty = true;
    // }
}
