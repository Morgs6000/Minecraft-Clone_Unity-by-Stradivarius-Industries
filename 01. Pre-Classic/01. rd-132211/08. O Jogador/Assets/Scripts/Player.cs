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

    public bool onGround = false;

    private new Transform camera;
    private float xRotation;

    private void Awake() {
        this.camera = GetComponentInChildren<Camera>().transform;
    }

    private void Start() {
        
    }

    private void Update() {
        Cursor.lockState = CursorLockMode.Locked;
        //this.MoveCameraToPlayer(0);
        //this.Render(0);
        this.UpdateCamera();

        this.Tick();
    }

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

    private void UpdateCamera() {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        /*
        float mouseX = 0.0f;
        float mouseY = 0.0f;

        if() {
            mouseX++;
        }
        if() {
            mouseX--;
        }
        if() {
            mouseY++;
        }
        if() {
            mouseY--;
        }
        */

        this.transform.Rotate(Vector3.up * mouseX);

        this.xRotation -= mouseY;
        this.xRotation = Mathf.Clamp(this.xRotation, -90, 90);

        this.camera.localRotation = Quaternion.Euler(this.xRotation, 0, 0);
    }

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
        float speed = 0.0f;

        if(this.onGround) {
            speed = 0.02f;
        }
        else {
            speed = 0.005f;
        }
        */

        Vector3 moveDirection = new Vector3(xa, 0.0f, za);
        moveDirection *= 4.317f;
        this.transform.Translate(moveDirection * Time.deltaTime);
    }

    public void Move(float xa, float ya, float za) {
        float xaOrg = xa;
        float yaOrg = ya;
        float zaOrg = za;


    }
}
