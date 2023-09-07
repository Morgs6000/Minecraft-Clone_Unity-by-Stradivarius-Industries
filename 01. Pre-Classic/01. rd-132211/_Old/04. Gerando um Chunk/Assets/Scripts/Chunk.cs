using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
    private Tesselator t = new Tesselator();

    private void Start() {
        this.Rebuild();
    }

    private void Update() {
        
    }

    private void Rebuild() {
        for(int x = 0; x < 16; x++) {
            for(int y = 0; y < 16; y++) {
                for(int z = 0; z < 16; z++) {
                    Tile.rock.Render(t, x, y, z);
                }
            }
        }
        
        this.t.Flush(gameObject.AddComponent<MeshFilter>(), gameObject.AddComponent<MeshRenderer>());
    }
}
