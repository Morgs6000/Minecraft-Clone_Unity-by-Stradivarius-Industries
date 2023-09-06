using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRenderer : MonoBehaviour {
    private int xChunks;
    private int yChunks;
    private int zChunks;

    private void Start() {
        this.Init();
    }

    private void Update() {
        
    }

    public void Init() {
        this.xChunks = 1;
        this.yChunks = 1;
        this.zChunks = 1;

        for(int x = 0; x < this.xChunks; x++) {
            for(int y = 0; y < this.yChunks; y++) {
                for(int z = 0; z < this.zChunks; z++) {

                }
            }
        }
    }
}
