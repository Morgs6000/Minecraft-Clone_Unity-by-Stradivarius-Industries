using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    /*
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    private int vertexIndex;
    */

    private Tesselator t = new Tesselator();

    private void Start() {
        /*
        this.Render();
        this.MeshGen();
        */

        this.Render(this.t, 0, 0, 0);
        this.t.Flush(gameObject.AddComponent<MeshFilter>());
        gameObject.AddComponent<MeshRenderer>();
    }

    private void Update() {
        
    }

    public void Render(Tesselator t, int x, int y, int z) {
        float x0 = x + 0.0f;
        float y0 = y + 0.0f;
        float z0 = z + 0.0f;

        float x1 = x + 1.0f;
        float y1 = y + 1.0f;
        float z1 = z + 1.0f;

        t.Vertex(x1, y0, z0);
        t.Vertex(x1, y1, z0);
        t.Vertex(x1, y1, z1);
        t.Vertex(x1, y0, z1);

        //t.Triangles();
    }

    /*
    public void Render() {
        this.vertices.Add(new Vector3(1, 0, 0));
        this.vertices.Add(new Vector3(1, 1, 0));
        this.vertices.Add(new Vector3(1, 1, 1));
        this.vertices.Add(new Vector3(1, 0, 1));

        // Primeiro Triangulo
        this.triangles.Add(0 + this.vertexIndex);
        this.triangles.Add(1 + this.vertexIndex);
        this.triangles.Add(2 + this.vertexIndex);

        // Segundo Triangulo
        this.triangles.Add(0 + this.vertexIndex);
        this.triangles.Add(2 + this.vertexIndex);
        this.triangles.Add(3 + this.vertexIndex);

        this.vertexIndex += 4;
    }

    private void MeshGen() {
        Mesh mesh = new Mesh();
        mesh.name = "Tile";

        mesh.vertices = this.vertices.ToArray();
        mesh.triangles = this.triangles.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>();
    }
    */
}
