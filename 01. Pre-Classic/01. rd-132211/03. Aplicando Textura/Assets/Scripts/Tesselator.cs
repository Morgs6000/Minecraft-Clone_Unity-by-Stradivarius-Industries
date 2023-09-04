using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tesselator {
    private List<Vector3> vertexBuffer = new List<Vector3>();
    private List<int> triangleBuffer = new List<int>();
    private List<Vector2> texCoordBuffer = new List<Vector2>();

    private int vertices;

    private float u;
    private float v;

    private bool hasTexture = false;

    public void Flush(MeshFilter meshFilter, MeshRenderer meshRenderer) {
        // Cria uma malha vazia
        Mesh mesh = new Mesh();
        //mesh.name = "Tile";
        
        // Define os vértices e índices da malha com base nos buffers
        mesh.vertices = this.vertexBuffer.ToArray();
        mesh.triangles = this.triangleBuffer.ToArray();
        mesh.uv = this.texCoordBuffer.ToArray();

        // Calcula informações adicionais para a malha
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.Optimize();

        // Define a malha gerada no MeshFilter para exibição no Unity
        meshFilter.mesh = mesh;
        meshRenderer.material = Resources.Load<Material>("Materials/Terrain");
    }

    public void Tex(float u, float v) {
        this.hasTexture = true;
        this.u = u;
        this.v = v;
    }

    public void Vertex(float x, float y, float z) {
        // Adiciona um vértice com as coordenadas (x, y, z) ao buffer
        this.vertexBuffer.Add(new Vector3(x, y, z));

        if(this.hasTexture) {
            this.texCoordBuffer.Add(new Vector2(u, v));
        }

        // Adiciona os índices dos triângulos para formar um quadrado
        this.vertices++;

        // Verifica se há um conjunto completo de 4 vértices para formar um quadrado
        if(this.vertices % 4 == 0) {
            // Calcula o índice inicial do conjunto de vértices
            int startIndex = this.vertices - 4;
            
            // Adiciona os índices dos triângulos para formar um quadrado
            // Primeiro Triangulo
            this.triangleBuffer.Add(0 + startIndex);
            this.triangleBuffer.Add(1 + startIndex);
            this.triangleBuffer.Add(2 + startIndex);

            // Segundo Triangulo
            this.triangleBuffer.Add(0 + startIndex);
            this.triangleBuffer.Add(2 + startIndex);
            this.triangleBuffer.Add(3 + startIndex);
        }
    }
}
