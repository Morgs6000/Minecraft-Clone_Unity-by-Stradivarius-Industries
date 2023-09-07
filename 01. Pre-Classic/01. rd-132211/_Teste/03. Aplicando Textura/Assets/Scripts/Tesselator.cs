using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tesselator {
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uv = new List<Vector2>();

    public int vertexIndex;

    public void MeshGen(MeshFilter meshFilter, MeshRenderer meshRenderer) {
        Mesh mesh = new Mesh();
        //mesh.name = "Chunk";
        
        mesh.vertices = this.vertices.ToArray();
        mesh.triangles = this.triangles.ToArray();
        mesh.uv = this.uv.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.Optimize();

        meshFilter.mesh = mesh;
        meshRenderer.material = Resources.Load<Material>("Materials/Terrain");
    }

    public void SetVertices(float x, float y, float z) {
        this.vertices.Add(new Vector3(x, y, z));
    }

    public void SetTriangles(int vertexIndex) {
        this.triangles.Add(vertexIndex);
    }

    public void SetUV(float u, float v) {
        this.uv.Add(new Vector2(u, v));
    }
}
