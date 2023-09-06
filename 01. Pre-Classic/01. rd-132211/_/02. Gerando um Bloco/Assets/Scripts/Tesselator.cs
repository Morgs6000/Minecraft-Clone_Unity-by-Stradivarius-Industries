using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tesselator {
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    public int vertexIndex;

    public void MeshGen(MeshFilter meshFilter) {
        Mesh mesh = new Mesh();
        //mesh.name = "Chunk";
        
        mesh.vertices = this.vertices.ToArray();
        mesh.triangles = this.triangles.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.Optimize();

        meshFilter.mesh = mesh;
    }

    public void SetVertices(float x, float y, float z) {
        this.vertices.Add(new Vector3(x, y, z));
    }

    public void SetTriangles(int vertexIndex) {
        this.triangles.Add(vertexIndex);
    }
}
