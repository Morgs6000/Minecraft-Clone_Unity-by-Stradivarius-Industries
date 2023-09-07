using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
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

    //public AABB bb;

    public bool onGround = false;

    private void Start() {
        
    }

    private void Update() {
        
    }

    /*
    public Player(Level level) {
        this.level = level;
        this.ResetPos();
    }
    */

    private void ResetPos() {
        float x = Random.Range(0, this.level.width);
        float y = (this.level.height + 10);
        float z = Random.Range(0, this.level.depth);
        this.SetPos(x, y, z);
    }

    private void SetPos(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
        /*
        float w = 0.3F;
        float h = 0.9F;
        this.bb = new AABB(x - w, y - h, z - w, x + w, y + h, z + w);
        */
    }

    public void Turn(float xo, float yo) {
        this.yRot = (this.yRot + xo * 0.15f);
        this.xRot = (this.xRot - yo * 0.15f);
        
        if (this.xRot < -90.0F) {
            this.xRot = -90.0F;
        }
        if (this.xRot > 90.0F) {
            this.xRot = 90.0F;
        }
    }

    /*
    private void MoveCameraToPlayer(float a) {
        this.camera.Translate(0.0f, 0.0f, -0.3f);
        this.camera.Rotate(this.xRot, 0.0f, 0.0f);
        this.camera.Rotate(0.0f, this.yRot, 0.0f);

        float x = this.xo + (this.x - this.xo) * a;
        float y = this.yo + (this.y - this.yo) * a;
        float z = this.zo + (this.z - this.zo) * a;

        this.camera.Translate(-x, -y, -z);
    }

    private void Render(float a) {
        float xo = Input.GetAxis("Mouse X");
        float yo = Input.GetAxis("Mouse Y");

        this.Turn(xo, yo);
    }
    */

    public void Tick() {
        float xa = 0.0f;
        float za = 0.0f;

        if(Input.GetKey(KeyCode.R)) {
            this.ResetPos();
        }

        if(Input.GetKey(KeyCode.W)) {
            za++;
        }
        if(Input.GetKey(KeyCode.S)) {
            za--;
        }
        if(Input.GetKey(KeyCode.A)) {
            xa--;
        }
        if(Input.GetKey(KeyCode.D)) {
            xa++;
        }

        if(Input.GetKey(KeyCode.Space) && this.onGround) {
            this.yd = 0.12f;
        }

        /*
        this.moveRelative(xa, ya, this.onGround ? 0.02F : 0.005F);
        this.yd = (float)((double)this.yd - 0.005);
        this.move(this.xd, this.yd, this.zd);
        this.xd *= 0.91F;
        this.yd *= 0.98F;
        this.zd *= 0.91F;
        if (this.onGround) {
            this.xd *= 0.8F;
            this.zd *= 0.8F;
        }
        */
    }

    public void Move(float xa, float ya, float za) {
        float xaOrg = xa;
        float yaOrg = ya;
        float zaOrg = za;

        /*
        List<AABB> aABBs = this.level.getCubes(this.bb.expand(xa, ya, za));

        int i;
        for(i = 0; i < aABBs.size(); ++i) {
            ya = ((AABB)aABBs.get(i)).clipYCollide(this.bb, ya);
        }

        this.bb.move(0.0F, ya, 0.0F);

        for(i = 0; i < aABBs.size(); ++i) {
            xa = ((AABB)aABBs.get(i)).clipXCollide(this.bb, xa);
        }

        this.bb.move(xa, 0.0F, 0.0F);

        for(i = 0; i < aABBs.size(); ++i) {
            za = ((AABB)aABBs.get(i)).clipZCollide(this.bb, za);
        }

        this.bb.move(0.0F, 0.0F, za);
        this.onGround = yaOrg != ya && yaOrg < 0.0F;
        if (xaOrg != xa) {
            this.xd = 0.0F;
        }

        if (yaOrg != ya) {
            this.yd = 0.0F;
        }

        if (zaOrg != za) {
            this.zd = 0.0F;
        }

        this.x = (this.bb.x0 + this.bb.x1) / 2.0F;
        this.y = this.bb.y0 + 1.62F;
        this.z = (this.bb.z0 + this.bb.z1) / 2.0F;
        */
    }

    /*
    public void moveRelative(float xa, float za, float speed) {
        float dist = xa * xa + za * za;
        if (!(dist < 0.01F)) {
            dist = speed / (float)Math.sqrt((double)dist);
            xa *= dist;
            za *= dist;
            float sin = (float)Math.sin((double)this.yRot * Math.PI / 180.0);
            float cos = (float)Math.cos((double)this.yRot * Math.PI / 180.0);
            this.xd += xa * cos - za * sin;
            this.zd += za * cos + xa * sin;
        }
    }
    */

    #if UNITY_EDITOR

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        Vector3 centroDoCubo = transform.position + new Vector3(0.0f, 0.9f, 0.0f);
        Vector3 tamanhoDoCubo = new Vector3(0.6f, 1.8f, 0.6f);

        Gizmos.DrawWireCube(centroDoCubo, tamanhoDoCubo);
    }

    #endif
}
