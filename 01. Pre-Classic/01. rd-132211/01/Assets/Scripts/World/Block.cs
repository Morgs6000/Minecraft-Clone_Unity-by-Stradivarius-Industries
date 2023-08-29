using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {
    public static List<Block> blocks = new List<Block>();

    public static Block AIR = new Block();
    public static Block STONE = new Block();
    public static Block GRASS = new Block();

    private bool isTransparent;

    private Block() {
        blocks.Add(this);
    }

    public bool IsBlockTransparent() {
        return this.isTransparent;
    }

    // Retorna os vértices das faces do bloco em um array 2D de Vector3
    public static Vector3[,] GetVertices() {
        return new Vector3[,] {
            /* RIGHT */ {            
                new Vector3(1, 0, 0),
                new Vector3(1, 1, 0),
                new Vector3(1, 1, 1),
                new Vector3(1, 0, 1)
            },
            /* LEFT */ {
                new Vector3(0, 0, 1),
                new Vector3(0, 1, 1),
                new Vector3(0, 1, 0),
                new Vector3(0, 0, 0)
            },        
            /* TOP */ {
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 1),
                new Vector3(1, 1, 1),
                new Vector3(1, 1, 0)
            },        
            /* BOTTOM */ {
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 0)
            },        
            /* FRONT */ {
                new Vector3(1, 0, 1),
                new Vector3(1, 1, 1),
                new Vector3(0, 1, 1),
                new Vector3(0, 0, 1)
            },        
            /* BACK */ {
                new Vector3(0, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(1, 1, 0),
                new Vector3(1, 0, 0)
            }
        };
    }

    // Retorna os índices dos vértices que formam os triângulos das faces do bloco
    public static int[] GetTriangles() {
        return new int[] {
            // Primeiro Triangulo
            0, 1, 2,
            
            // Segundo Triangulo
            0, 2, 3
        };
    }

    // Define as coordenadas de textura (UV) para mapear a textura no bloco
    public static Vector2[] SetUV(Vector2 textureCoordinate) {
        // Define o tamanho da textura em "tiles"
        Vector2 textureSizeInTiles = new Vector2(16, 16);
        
        // Inverte as coordenadas y para se adequar à convenção de texturas
        float y = (textureSizeInTiles.y - 1) - textureCoordinate.y;

        // Calcula o tamanho de um "tile" em termos de coordenadas UV
        float tileSizeU = 1.0f / textureSizeInTiles.x;
        float tileSizeV = 1.0f / textureSizeInTiles.y;

        // Calcula as coordenadas UV para os cantos do retângulo
        float x0 = textureCoordinate.x * tileSizeU;
        float x1 = x0 + tileSizeU;
        float y0 = y * tileSizeV;
        float y1 = y0 + tileSizeV;

        // Retorna as coordenadas UV para mapeamento de textura
        return new Vector2[] {
            new Vector2(x0, y0),
            new Vector2(x0, y1),
            new Vector2(x1, y1),
            new Vector2(x1, y0)
        };
    }
}
