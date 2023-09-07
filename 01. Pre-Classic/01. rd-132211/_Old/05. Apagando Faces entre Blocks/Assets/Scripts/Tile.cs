using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
    public static Tile rock = new Tile(new Vector2(1, 0));
    public static Tile grass = new Tile(new Vector2(0, 0));

    private Vector2 tex = new Vector2(0, 0);

    private Tile(Vector2 tex) {
        this.tex = tex;
    }

    public void Render(Tesselator t, Level level, int x, int y, int z) {
        float u0 = this.tex.x * (1.0f / 16.0f);
        float u1 = u0 + (1.0f / 16.0f);
        float v0 = ((16.0f - 1.0f) - this.tex.y) * (1.0f / 16.0f);
        float v1 = v0 + (1.0f / 16.0f);

        float x0 = x + 0.0f;
        float x1 = x + 1.0f;

        float y0 = y + 0.0f;
        float y1 = y + 1.0f;

        float z0 = z + 0.0f;
        float z1 = z + 1.0f;

        // RIGHT
        if(!level.IsSolidTile(x + 1, y, z)) {
            t.Tex(u0, v0);
            t.Vertex(x1, y0, z0);
            t.Tex(u0, v1);
            t.Vertex(x1, y1, z0);
            t.Tex(u1, v1);
            t.Vertex(x1, y1, z1);
            t.Tex(u1, v0);
            t.Vertex(x1, y0, z1);
        }

        // LEFT
        if(!level.IsSolidTile(x - 1, y, z)) {
            t.Tex(u0, v0);
            t.Vertex(x0, y0, z1);
            t.Tex(u0, v1);
            t.Vertex(x0, y1, z1);
            t.Tex(u1, v1);
            t.Vertex(x0, y1, z0);
            t.Tex(u1, v0);
            t.Vertex(x0, y0, z0);
        }

        // TOP
        if(!level.IsSolidTile(x, y + 1, z)) {
            t.Tex(u0, v0);
            t.Vertex(x0, y1, z0);
            t.Tex(u0, v1);
            t.Vertex(x0, y1, z1);
            t.Tex(u1, v1);
            t.Vertex(x1, y1, z1);
            t.Tex(u1, v0);
            t.Vertex(x1, y1, z0);
        }

        // BOTTOM
        if(!level.IsSolidTile(x, y - 1, z)) {
            t.Tex(u0, v0);
            t.Vertex(x1, y0, z0);
            t.Tex(u0, v1);
            t.Vertex(x1, y0, z1);
            t.Tex(u1, v1);
            t.Vertex(x0, y0, z1);
            t.Tex(u1, v0);
            t.Vertex(x0, y0, z0);
        }

        // FRONT
        if(!level.IsSolidTile(x, y, z + 1)) {
            t.Tex(u0, v0);
            t.Vertex(x1, y0, z1);
            t.Tex(u0, v1);
            t.Vertex(x1, y1, z1);
            t.Tex(u1, v1);
            t.Vertex(x0, y1, z1);
            t.Tex(u1, v0);
            t.Vertex(x0, y0, z1);
        }

        // BACK
        if(!level.IsSolidTile(x, y, z - 1)) {
            t.Tex(u0, v0);
            t.Vertex(x0, y0, z0);
            t.Tex(u0, v1);
            t.Vertex(x0, y1, z0);
            t.Tex(u1, v1);
            t.Vertex(x1, y1, z0);
            t.Tex(u1, v0);
            t.Vertex(x1, y0, z0);
        }
    }
}
