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

    public CharacterController characterController;

    private float speed;
    private float walking = 4.317f;
    private float falling = -78.4f;
    private float jumpHeight = 1.2522f;

    //private bool onGround = false;

    private Vector3 velocity;

    private float rangeHit = 5.0f;

    private void Awake() {
        this.camera = GetComponentInChildren<Camera>().transform;
        this.characterController = GetComponent<CharacterController>();
    }

    private void Start() {
        this.ResetPos();

        this.speed = this.walking;
    }

    private void Update() {
        Cursor.lockState = CursorLockMode.Locked;
        //this.MoveCameraToPlayer(0);
        //this.Render(0);
        this.UpdateCamera();

        this.Tick();
        this.UpdateFalling();
        this.UpdateJump();

        this.UpdateRaycast();

        this.InputQuitGame();
        this.InputSaveGame();
    }

    private void ResetPos() {
        //float x = Random.Range(0, this.level.width);
        float x = Random.Range(0, 256);
        //float y = (this.level.height + 10);
        float y = (64 + 10);
        //float z = Random.Range(0, this.level.depth);
        float z = Random.Range(0, 256);
        //this.SetPos(x, y, z);

        transform.position = new Vector3(x, y, z);
    }

    /*
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
    */

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

        /*
        if(Input.GetKey(KeyCode.Space) && this.onGround) {
            this.yd = 0.12f;
        }
        */

        /*
        float speed = 0.0f;

        if(this.onGround) {
            speed = 0.02f;
        }
        else {
            speed = 0.005f;
        }
        */

        //Vector3 moveDirection = new Vector3(xa, 0.0f, za);
        Vector3 moveDirection = transform.TransformDirection(xa, 0.0f, za);
        moveDirection *= this.speed;
        //this.transform.Translate(moveDirection * Time.deltaTime);        
        this.characterController.Move(moveDirection * Time.deltaTime);
    }

    /*
    public void Move(float xa, float ya, float za) {
        float xaOrg = xa;
        float yaOrg = ya;
        float zaOrg = za;


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

    private GameObject highlight;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private Material material;

    //private LayerMask groundMaks = LayerMask.GetMask("Ground");

    private void UpdateRaycast() {
        RaycastHit hit;

        if(Physics.Raycast(this.camera.position, this.camera.forward, out hit, this.rangeHit, LayerMask.GetMask("Ground"))) {
            Vector3 pointPos = hit.point - hit.normal / 2;
            Vector3 pointPos2 = hit.point + hit.normal / 2;

            if(!this.highlight) {
                this.CreateHighlight();
            }

            this.highlight.transform.position = new Vector3(
                Mathf.FloorToInt(pointPos.x),
                Mathf.FloorToInt(pointPos.y),
                Mathf.FloorToInt(pointPos.z)
            );

            this.highlight.SetActive(true);
            this.UpdateHighlight(pointPos, pointPos2);
            this.UpdateColor();

            if(Input.GetMouseButtonDown(1)) {
                /*
                Chunk c = World.GetChunk(new Vector3(
                    Mathf.FloorToInt(pointPos.x),
                    Mathf.FloorToInt(pointPos.y),
                    Mathf.FloorToInt(pointPos.z)
                ));

                c.SetBlock(pointPos, Block.AIR.blockID);
                */
            }
            if(Input.GetMouseButtonDown(0)) {
                /*
                float distance = 0.81f;
                float playerDistance = Vector3.Distance(transform.position, pointPos);
                float camDistance = Vector3.Distance(camera.position, pointPos);

                if(playerDistance < distance || camDistance < distance) {
                    return;
                }
                if(pointPos.y > World.worldSizeInBlocks.y) {
                    //WarningMensage();
                    
                    return;
                }

                Chunk c = World.GetChunk(new Vector3(
                    Mathf.FloorToInt(pointPos2.x),
                    Mathf.FloorToInt(pointPos2.y),
                    Mathf.FloorToInt(pointPos2.z)
                ));

                c.SetBlock(pointPos2, Block.STONE.blockID);
                */
            }
        }
        else {
            if(this.highlight) {
                this.highlight.SetActive(false);
            }
        }
    }

    private void CreateHighlight() {
        this.highlight = new GameObject("Block Highlight");

        this.meshFilter = this.highlight.AddComponent<MeshFilter>();
        this.meshRenderer = this.highlight.AddComponent<MeshRenderer>();
        
        this.material = Resources.Load<Material>("Materials/Block Highlight");
    }

    private void UpdateHighlight(Vector3 pointPos, Vector3 pointPos2) {
        Mesh mesh = new Mesh();
        mesh.name = "Block Highlight";

        this.vertices.Clear();
        this.triangles.Clear();

        /*
        for(int verts = 0; verts < 4; verts++) {
            if(pointPos2.x > pointPos.x) {
                this.vertices.Add(Block.GetVertices()[0, verts]);
            }
            if(pointPos2.x < pointPos.x) {
                this.vertices.Add(Block.GetVertices()[1, verts]);
            }
            if(pointPos2.y > pointPos.y) {
                this.vertices.Add(Block.GetVertices()[2, verts]);
            }
            if(pointPos2.y < pointPos.y) {
                this.vertices.Add(Block.GetVertices()[3, verts]);
            }
            if(pointPos2.z > pointPos.z) {
                this.vertices.Add(Block.GetVertices()[4, verts]);
            }
            if(pointPos2.z < pointPos.z) {
                this.vertices.Add(Block.GetVertices()[5, verts]);
            }
        }
        for(int tris = 0; tris < 6; tris++) {
            this.triangles.Add(Block.GetTriangles()[tris]);
        }
        */

        if(pointPos2.x > pointPos.x) {
            this.vertices.Add(new Vector3(1, 0, 0));
            this.vertices.Add(new Vector3(1, 1, 0));
            this.vertices.Add(new Vector3(1, 1, 1));
            this.vertices.Add(new Vector3(1, 0, 1));
        }
        if(pointPos2.x < pointPos.x) {
            this.vertices.Add(new Vector3(0, 0, 1));
            this.vertices.Add(new Vector3(0, 1, 1));
            this.vertices.Add(new Vector3(0, 1, 0));
            this.vertices.Add(new Vector3(0, 0, 0));
        }
        if(pointPos2.y > pointPos.y) {
            this.vertices.Add(new Vector3(0, 1, 0));
            this.vertices.Add(new Vector3(0, 1, 1));
            this.vertices.Add(new Vector3(1, 1, 1));
            this.vertices.Add(new Vector3(1, 1, 0));
        }
        if(pointPos2.y < pointPos.y) {
            this.vertices.Add(new Vector3(1, 0, 0));
            this.vertices.Add(new Vector3(1, 0, 1));
            this.vertices.Add(new Vector3(0, 0, 1));
            this.vertices.Add(new Vector3(0, 0, 0));
        }
        if(pointPos2.z > pointPos.z) {
            this.vertices.Add(new Vector3(1, 0, 1));
            this.vertices.Add(new Vector3(1, 1, 1));
            this.vertices.Add(new Vector3(0, 1, 1));
            this.vertices.Add(new Vector3(0, 0, 1));
        }
        if(pointPos2.z < pointPos.z) {
            this.vertices.Add(new Vector3(0, 0, 0));
            this.vertices.Add(new Vector3(0, 1, 0));
            this.vertices.Add(new Vector3(1, 1, 0));
            this.vertices.Add(new Vector3(1, 0, 0));
        }
        
        // Primeiro Triangulo
        this.triangles.Add(0);
        this.triangles.Add(1);
        this.triangles.Add(2);

        // Segundo Triangulo
        this.triangles.Add(0);
        this.triangles.Add(2);
        this.triangles.Add(3);

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.Optimize();

        this.meshFilter.mesh = mesh;
        this.meshRenderer.material = material;
    }

    private void UpdateColor() {
        Color color = this.material.color;
        color.a = Mathf.PingPong(Time.time * 2.0f, 1) / 2;
        this.material.color = color;
    }

    private void InputQuitGame() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            //SaveHandler.Save();
            Application.Quit();
        }
    }

    private void InputSaveGame() {
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
            //SaveHandler.Save();
        }
    }
}
