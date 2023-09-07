using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    private Tesselator t = new Tesselator();

    private void Start() {
        this.Render(this.t, 0, 0, 0);
        this.t.Flush(gameObject.AddComponent<MeshFilter>());
        gameObject.AddComponent<MeshRenderer>();
    }

    public void Render(Tesselator t, int x, int y, int z) {
        float x0 = x + 0.0f;
        float y0 = y + 0.0f;
        float z0 = z + 0.0f;

        float x1 = x + 1.0f;
        float y1 = y + 1.0f;
        float z1 = z + 1.0f;

        // RIGHT
        t.Vertex(x1, y0, z0);
        t.Vertex(x1, y1, z0);
        t.Vertex(x1, y1, z1);
        t.Vertex(x1, y0, z1);

        // LEFT
        t.Vertex(x0, y0, z1);
        t.Vertex(x0, y1, z1);
        t.Vertex(x0, y1, z0);
        t.Vertex(x0, y0, z0);

        // TOP
        t.Vertex(x0, y1, z0);
        t.Vertex(x0, y1, z1);
        t.Vertex(x1, y1, z1);
        t.Vertex(x1, y1, z0);

        // BOTTOM
        t.Vertex(x1, y0, z0);
        t.Vertex(x1, y0, z1);
        t.Vertex(x0, y0, z1);
        t.Vertex(x0, y0, z0);

        // FRONT
        t.Vertex(x1, y0, z1);
        t.Vertex(x1, y1, z1);
        t.Vertex(x0, y1, z1);
        t.Vertex(x0, y0, z1);

        // BACK
        t.Vertex(x0, y0, z0);
        t.Vertex(x0, y1, z0);
        t.Vertex(x1, y1, z0);
        t.Vertex(x1, y0, z0);
    }
}
