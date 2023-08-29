using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
    private Level level;

    public float xo;
    public float yo;
    public float zo;

    public float x;
    public float y;
    public float z;

    public float xd;
    public float yd;
    public float zd;

    public float yRot;
    public float xRot;

    public bool onGround = false;

    public Player(Level level) {
        this.level = level;
        
    }

    public void ResetPos() {
        
    }

    private void SetPos(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;

        float w = 0.3f;
        float h = 0.9f;
    }

    public void Turn(float xo, float yo) {
        this.yRot = (float)((double)this.yRot + (double)xo * 0.15f);
        this.xRot = (float)((double)this.xRot + (double)yo * 0.15f);

        if(this.xRot < -90.0f) {
            this.xRot = -90.0f;
        }
        if(this.xRot > 90.0f) {
            this.xRot = 90.0f;
        }
    }

    public void Tick() {
        this.xo = this.x;
        this.yo = this.y;
        this.zo = this.z;

        float xa = 0.0f;
        float ya = 0.0f;

        if(Input.GetKeyDown(KeyCode.R)) {
            this.ResetPos();
        }

        if(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.W)) {
            ya--;
        }
        if(Input.GetKeyDown(KeyCode.RightAlt) || Input.GetKeyDown(KeyCode.S)) {
            ya++;
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            xa--;
        }
        if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            xa++;
        }
        if(Input.GetKeyDown(KeyCode.Space) && this.onGround) {
            this.yd = 0.12f;
        }

        this.MoveRelative(xa, ya, this.onGround ? 0.02f : 0.005f);

        this.yd = (float)((double) this.yd - 0.005f);

        
    }

    public void Move(float xa, float ya, float za) {
        float xaOrg = xa;
        float yaOrg = ya;
        float zaOrg = za;

        int i;
    }

    public void MoveRelative(float xa, float za, float speed) {
        float dist = xa * xa + za * za;

        if(!(dist < 0.01f)) {
            dist = speed / (float)Math.Sqrt((double)dist);

            xa *= dist;
            za *= dist;

            float sin = (float)Math.Sin((double)this.yRot * Math.PI / 180.0f);
            float cos = (float)Math.Cos((double)this.yRot * Math.PI / 180.0f);

            this.xd += xa * cos - za * sin;
            this.zd += za * cos + za * sin;
        }
    }
}
