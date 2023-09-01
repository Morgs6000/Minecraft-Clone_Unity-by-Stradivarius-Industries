using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private new Camera camera;
    private float xRotation;

    private CharacterController characterController;

    private float speed;
    private float walking = 4.317f;
    //private float sprinting = 5.612f;
    private float falling = -78.4f;
    private float jumpHeight = 1.2522f;

    private bool onGround = false;
    //private bool isSprinting;

    private Vector3 velocity;

    //private float lastClickTime;
    //private float DOUBLE_CLICK_TIME = 0.2f;

    private void Awake() {
        this.camera = GetComponentInChildren<Camera>();
        this.characterController = GetComponent<CharacterController>();
    }

    private void Start() {
        this.speed = this.walking;

        this.ResetPos();
    }

    private void Update() {
        Cursor.lockState = CursorLockMode.Locked;
        this.UpdateCamera();

        this.UpdateMovement();
        this.UpdateFalling();
        this.UpdateJump();
        //this.UpdateSprint();
    }

    private void ResetPos() {
        Vector3 worldSize = new Vector3(
            (World.worldSizeInBlocks.x - 1) / 2,
            World.worldSizeInBlocks.y,
            (World.worldSizeInBlocks.z - 1) / 2
        );

        float x = Random.Range(-worldSize.x, worldSize.x);
        float y = worldSize.y + 10.0f;
        float z = Random.Range(-worldSize.z, worldSize.z);

        transform.position = new Vector3(x, y, z);
    }

    private void UpdateCamera() {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up * mouseX);

        this.xRotation -= mouseY;
        this.xRotation = Mathf.Clamp(this.xRotation, -90, 90);

        this.camera.transform.localRotation = Quaternion.Euler(this.xRotation, 0, 0);
    }

    private void UpdateMovement() {
        float x = 0.0f;
        float z = 0.0f;

        if(Input.GetKey(KeyCode.W)) {
            z++;
        }
        if(Input.GetKey(KeyCode.S)) {
            z--;
        }
        if(Input.GetKey(KeyCode.A)) {
            x--;
        }
        if(Input.GetKey(KeyCode.D)) {
            x++;
        }

        Vector3 moveDirection = transform.TransformDirection(new Vector3(x, 0.0f, z));

        moveDirection *= this.speed;
        this.characterController.Move(moveDirection * Time.deltaTime);
    }

    private void UpdateFalling() {
        this.velocity.y += this.falling * Time.deltaTime;
        this.characterController.Move(velocity * Time.deltaTime);

        this.onGround = this.characterController.isGrounded;

        if(this.onGround && this.velocity.y < 0) {
            this.velocity.y = -2.0f;
        }
    }

    private void UpdateJump() {
        if(this.onGround && Input.GetKey(KeyCode.Space)) {
            this.onGround = false;

            this.velocity.y = Mathf.Sqrt(this.jumpHeight * -2.0f * this.falling);
        }
    }

    /*
    private void UpdateSprint() {
        if(Input.GetKey(KeyCode.LeftControl)) {
            this.isSprinting = true;
        }

        if(Input.GetKeyDown(KeyCode.W)) {
            float timeSinceLastClick = Time.time - this.lastClickTime;

            if(timeSinceLastClick <= this.DOUBLE_CLICK_TIME) {
                this.isSprinting = !this.isSprinting;
            }

            this.lastClickTime = Time.time;
        }
        if(Input.GetKeyUp(KeyCode.W)) {
            this.isSprinting = false;
        }

        if(isGrounded) {
            this.speed = this.isSprinting ? this.sprinting : this.walking;
        }
    }
    */
}
