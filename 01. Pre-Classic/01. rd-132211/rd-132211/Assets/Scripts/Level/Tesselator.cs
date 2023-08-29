using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tesselator {
    private List<Vector3> vertexBuffer = new List<Vector3>();
    private List<Vector2> texCoordBuffer = new List<Vector2>();

    private int vertices = 0;

    private float u;
    private float v;
    
    private bool hasTexture = false;

    public Tesselator() {

    }

    public void Flush() {
        Mesh mesh = new Mesh();

        mesh.vertices = vertexBuffer.ToArray();
        mesh.uv = texCoordBuffer.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.Optimize();

        this.Clear();
    }

    public void Clear() {
        this.vertices = 0;
    }

    public void Init() {
        this.Clear();

        this.hasTexture = false;
    }

    public void Tex(float u, float v) {
        this.hasTexture = true;

        this.u = u;
        this.v = v;
    }

    public void Vertex(float x, float y, float z) {
        this.vertexBuffer.Add(new Vector3(x, y, z));

        if(this.hasTexture) {
            this.texCoordBuffer.Add(new Vector2(u, v));
        }

        this.vertices++;
    }
}
