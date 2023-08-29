using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
    public static Tile rock = new Tile(0);
    public static Tile grass = new Tile(1);

    private int tex = 0;

    private Tile(int tex) {
        this.tex = tex;
    }

    public void Render(Tesselator t, Level level, int layer, int x, int y, int z) {
        float u0 = (float)this.tex / 16.0f;
        float u1 = u0 + (1.0f / 16.0f);
        float v0 = 0.0f;
        float v1 = v0 + (1.0f / 16.0f);

        float c1 = 1.0f;
        float c2 = 0.8f;
        float c3 = 0.6f;

        float x0 = (float)x + 0.0f;
        float x1 = (float)x + 1.0f;

        float y0 = (float)y + 0.0f;
        float y1 = (float)y + 1.0f;

        float z0 = (float)z + 0.0f;
        float z1 = (float)z + 1.0f;

        if(!level.IsSolidTile(x + 1, y, z)) {
            if(layer == 1) {
                t.Vertex(x1, y0, z0);
                t.Vertex(x1, y1, z0);
                t.Vertex(x1, y1, z1);
                t.Vertex(x1, y0, z1);

                t.Tex(u0, v0);
                t.Tex(u0, v1);
                t.Tex(u1, v1);
                t.Tex(u1, v0);
            }
        }
        if(!level.IsSolidTile(x - 1, y, z)) {
            if(layer == 1) {
                t.Vertex(x0, y0, z1);
                t.Vertex(x0, y1, z1);
                t.Vertex(x0, y1, z0);
                t.Vertex(x0, y0, z0);

                t.Tex(u0, v0);
                t.Tex(u0, v1);
                t.Tex(u1, v1);
                t.Tex(u1, v0);
            }
        }
        if(!level.IsSolidTile(x, y + 1, z)) {
            if(layer == 1) {
                t.Vertex(x0, y1, z0);
                t.Vertex(x0, y1, z1);
                t.Vertex(x1, y1, z1);
                t.Vertex(x1, y1, z0);

                t.Tex(u0, v0);
                t.Tex(u0, v1);
                t.Tex(u1, v1);
                t.Tex(u1, v0);
            }
        }
        if(!level.IsSolidTile(x, y - 1, z)) {
            if(layer == 1) {
                t.Vertex(x1, y0, z0);
                t.Vertex(x1, y0, z1);
                t.Vertex(x0, y0, z1);
                t.Vertex(x0, y0, z0);

                t.Tex(u0, v0);
                t.Tex(u0, v1);
                t.Tex(u1, v1);
                t.Tex(u1, v0);
            }
        }
        if(!level.IsSolidTile(x, y, z + 1)) {
            if(layer == 1) {
                t.Vertex(x1, y0, z1);
                t.Vertex(x1, y1, z1);
                t.Vertex(x0, y1, z1);
                t.Vertex(x0, y0, z1);

                t.Tex(u0, v0);
                t.Tex(u0, v1);
                t.Tex(u1, v1);
                t.Tex(u1, v0);
            }
        }
        if(!level.IsSolidTile(x, y, z - 1)) {
            if(layer == 1) {
                t.Vertex(x0, y0, z0);
                t.Vertex(x0, y1, z0);
                t.Vertex(x1, y1, z0);
                t.Vertex(x1, y0, z0);

                t.Tex(u0, v0);
                t.Tex(u0, v1);
                t.Tex(u1, v1);
                t.Tex(u1, v0);
            }
        }
    }

    public void RenderFace(Tesselator t, int x, int y, int z, int face) {
        float x0 = (float)x + 0.0f;
        float x1 = (float)x + 1.0f;

        float y0 = (float)y + 0.0f;
        float y1 = (float)y + 1.0f;

        float z0 = (float)x + 0.0f;
        float z1 = (float)x + 1.0f;

        if(face == 0) {
            t.Vertex(x1, y0, z0);
            t.Vertex(x1, y1, z0);
            t.Vertex(x1, y1, z1);
            t.Vertex(x1, y0, z1);
        }
        if(face == 1) {
            t.Vertex(x0, y0, z1);
            t.Vertex(x0, y1, z1);
            t.Vertex(x0, y1, z0);
            t.Vertex(x0, y0, z0);
        }
        if(face == 2) {
            t.Vertex(x0, y1, z0);
            t.Vertex(x0, y1, z1);
            t.Vertex(x1, y1, z1);
            t.Vertex(x1, y1, z0);
        }
        if(face == 3) {
            t.Vertex(x1, y0, z0);
            t.Vertex(x1, y0, z1);
            t.Vertex(x0, y0, z1);
            t.Vertex(x0, y0, z0);
        }
        if(face == 4) {
            t.Vertex(x1, y0, z1);
            t.Vertex(x1, y1, z1);
            t.Vertex(x0, y1, z1);
            t.Vertex(x0, y0, z1);
        }
        if(face == 5) {
            t.Vertex(x0, y0, z0);
            t.Vertex(x0, y1, z0);
            t.Vertex(x1, y1, z0);
            t.Vertex(x1, y0, z0);
        }
    }
}
