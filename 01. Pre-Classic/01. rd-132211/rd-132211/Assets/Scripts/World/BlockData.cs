using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockData {
    public static Vector3[,] vertices = {
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
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1)
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

    public static int[] triangles = {
        // Primeiro Triangulo
        0, 1, 2, 
        
        // Segundo Triangulo
        0, 2, 3
    };

    public static Vector2[] uv(Vector2 textureCoordinate) {
        Vector2 textureSizeInTiles = new Vector2(16, 16);
        
        float x = textureCoordinate.x;
        float y = textureCoordinate.y;

        float _x = 1.0f / textureSizeInTiles.x;
        float _y = 1.0f / textureSizeInTiles.y;

        y = (textureSizeInTiles.y - 1) - y;

        x *= _x;
        y *= _y;

        return new Vector2[] {
            new Vector2(x, y),
            new Vector2(x, y + _y),
            new Vector2(x + _x, y + _y),
            new Vector2(x + _x, y)
        };
    }

    public static Vector3[] blockSide = {
        new Vector3(1, 0, 0),  // RIGHT
        new Vector3(-1, 0, 0), // LEFT
        new Vector3(0, 1, 0),  // TOP
        new Vector3(0, -1, 0), // BOTTOM
        new Vector3(0, 0, 1),  // FRONT
        new Vector3(0, 0, -1)  // BACK
    };
}

public enum EnumBlockSide { RIGHT, LEFT, TOP, BOTTOM, FRONT, BACK }
