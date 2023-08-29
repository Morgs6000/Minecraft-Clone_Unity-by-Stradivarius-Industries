using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level {
    public int width;
    public int height;
    public int depth;

    private byte[] blocks;

    private List<LevelListener> levelListeners = new List<LevelListener>();

    public void AddListerner(LevelListener levelListener) {
        this.levelListeners.Add(levelListener);
    }

    public bool IsTile(int x, int y, int z) {
        if(
            x >= 0 && x < this.width &&
            y >= 0 && y < this.depth &&
            z >= 0 && z < this.height
        ) {
            return this.blocks[(y * this.height + z) * this.width + x] == 1;
        }
        else {
            return false;
        }
    }

    public bool IsSolidTile(int x, int y, int z) {
        return this.IsTile(x, y, z);
    }
}
