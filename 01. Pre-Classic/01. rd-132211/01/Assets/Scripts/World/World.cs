using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {
    private void Start() {
        WorldGen();
    }

    private void WorldGen() {
        Chunk chunk = new Chunk(transform);
    }
}
