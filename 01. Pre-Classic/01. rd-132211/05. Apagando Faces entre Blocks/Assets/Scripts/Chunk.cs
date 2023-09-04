using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
    private Level level;

    private Tesselator t = new Tesselator();

    private void Start() {
        this.level = new Level(16, 16, 16);
        this.Rebuild();
    }

    private void Update() {
        
    }

    private void Rebuild() {
        for(int x = 0; x < 16; x++) {
            for(int y = 0; y < 16; y++) {
                for(int z = 0; z < 16; z++) {
                    if(this.level.IsTile(x, y, z)) {
                        Tile.rock.Render(t, this.level, x, y, z);
                    }
                }
            }
        }
        
        this.t.Flush(gameObject.AddComponent<MeshFilter>(), gameObject.AddComponent<MeshRenderer>());
    }
}
