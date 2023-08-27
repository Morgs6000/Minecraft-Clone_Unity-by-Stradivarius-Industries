using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {
    public static List<Block> blocksList = new List<Block>();
    public static Block STONE = new Block().setUVCoord(1, 0);
    public static Block GRASS = new Block().setUVCoord(0, 0);
    
    private Vector2 blockUV;
    
    public Block() {
        blocksList.Add(this);
    }

    public Block setUVCoord(int x, int y) {
        this.blockUV = new Vector2(x, y);
        return this;
    }

    public Vector2 getUVCoord() {
        return this.blockUV;
    }

    public static void Render(Chunk chunk, Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;

        for(int side = 0; side < 6; side++) {
            if(!isSolidTile(chunk, BlockData.blockSide[side] + offset)) {
                for(int verts = 0; verts < 4; verts++) {
                    chunk.vertices.Add(BlockData.vertices[side, verts] + offset);
                    chunk.uv.Add(BlockData.uv(chunk.blocks[x, y, z].getUVCoord())[verts]);
                }
                for(int tris = 0; tris < 6; tris++) {
                    chunk.triangles.Add(BlockData.triangles[tris] + chunk.vertexIndex);
                }

                chunk.vertexIndex += 4;
            }
        }
    }

    public static bool isSolidTile(Chunk chunk, Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;

        if(
            x < 0 || x > Chunk.chunkSizeInBlocks.x - 1 ||
            y < 0 || y > Chunk.chunkSizeInBlocks.y - 1 ||
            z < 0 || z > Chunk.chunkSizeInBlocks.z - 1
        ) {
            return false;
        }
        if(chunk.blocks[x, y, z] == null) {
            return false;
        }

        return true;
    }
}
