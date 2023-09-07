using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    private Vector2 tex = new Vector2(0, 0);

    private Tesselator t = new Tesselator();

    private void Start() {
        this.BlockGen(this.t, 0, 0, 0);
        this.t.MeshGen(gameObject.AddComponent<MeshFilter>());
        gameObject.AddComponent<MeshRenderer>()
    }

    private void Update() {
        
    }

    public void BlockGen(Tesselator t, int x, int y, int z) {
        float x0 = x + 0.0f;
        float y0 = y + 0.0f;
        float z0 = z + 0.0f;

        float x1 = x + 1.0f;
        float y1 = y + 1.0f;
        float z1 = z + 1.0f;

        // RIGHT
        t.SetVertices(x1, y0, z0);
        t.SetVertices(x1, y1, z0);
        t.SetVertices(x1, y1, z1);
        t.SetVertices(x1, y0, z1);

        this.SetTriangles(t);

        // LEFT
        t.SetVertices(x0, y0, z1);
        t.SetVertices(x0, y1, z1);
        t.SetVertices(x0, y1, z0);
        t.SetVertices(x0, y0, z0);

        this.SetTriangles(t);

        // TOP
        t.SetVertices(x0, y1, z0);
        t.SetVertices(x0, y1, z1);
        t.SetVertices(x1, y1, z1);
        t.SetVertices(x1, y1, z0);

        this.SetTriangles(t);

        // BOTTOM
        t.SetVertices(x1, y0, z0);
        t.SetVertices(x1, y0, z1);
        t.SetVertices(x0, y0, z1);
        t.SetVertices(x0, y0, z0);

        this.SetTriangles(t);

        // FRONT
        t.SetVertices(x1, y0, z1);
        t.SetVertices(x1, y1, z1);
        t.SetVertices(x0, y1, z1);
        t.SetVertices(x0, y0, z1);

        this.SetTriangles(t);

        // BACK
        t.SetVertices(x0, y0, z0);
        t.SetVertices(x0, y1, z0);
        t.SetVertices(x1, y1, z0);
        t.SetVertices(x1, y0, z0);

        this.SetTriangles(t);
    }

    public void SetTriangles(Tesselator t) {
        int t0 = 0 + t.vertexIndex;
        int t1 = 1 + t.vertexIndex;
        int t2 = 2 + t.vertexIndex;
        int t3 = 3 + t.vertexIndex;

        // Primeiro Triangulo
        t.SetTriangles(t0);
        t.SetTriangles(t1);
        t.SetTriangles(t2);

        // Segundo Triangulo
        t.SetTriangles(t0);
        t.SetTriangles(t2);
        t.SetTriangles(t3);

        t.vertexIndex += 4;
    }
}
