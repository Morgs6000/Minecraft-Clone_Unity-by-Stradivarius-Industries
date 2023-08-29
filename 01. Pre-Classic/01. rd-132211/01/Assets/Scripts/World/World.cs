using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {
    public Vector3 worldSizeInBlocks = new Vector3(256, 64, 256);

    private void Start() {
        WorldGen();
    }

    private void WorldGen() {
        Chunk chunk = new Chunk(transform);
    }
}
