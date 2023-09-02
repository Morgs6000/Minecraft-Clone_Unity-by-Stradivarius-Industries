using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class Player : MonoBehaviour {
    private new Transform camera;
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

    private float rangeHit = 5.0f;

    //private float lastClickTime;
    //private float DOUBLE_CLICK_TIME = 0.2f;

    private void Awake() {
        //this.camera = GetComponentInChildren<Camera>();
        //this.characterController = GetComponent<CharacterController>();
    }

    private void Start() {
        //this.speed = this.walking;

        //this.ResetPos();
    }

    private void Update() {
        Cursor.lockState = CursorLockMode.Locked;
        this.UpdateCamera();

        this.UpdateMovement();
        this.UpdateFalling();
        this.UpdateJump();
        //this.UpdateSprint();

        if(Input.GetKey(KeyCode.R)) {
            this.ResetPos();
        }

        this.UpdateRaycast();

        this.InputQuitGame();
        //this.InputFullScreen();
    }

    /*
    private bool isFullScreen = false;
    
    private void InputFullScreen() {
        if(Input.GetKeyDown(KeyCode.F11)) {
            this.isFullScreen = !this.isFullScreen;
            Screen.fullScreen = this.isFullScreen;
        }
    }
    */

    private void InputSaveGame() {
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
            
        }
    }

    private void InputQuitGame() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    public void Init() {   
        //GameObject newPlayer = new GameObject();
        //Player player = gameObject.AddComponent<Player>();

        this.InitCamera();
        this.InitCharacterController();

        this.speed = this.walking;

        this.ResetPos();
    }

    private void InitCamera() {
        this.camera = new GameObject("Camera").AddComponent<Camera>().transform;
        this.camera.gameObject.transform.parent = transform;
        this.camera.transform.position = new Vector3(0.0f, 1.62f, 0.0f);
    }

    private void InitCharacterController() {
        this.characterController = gameObject.AddComponent<CharacterController>();

        this.characterController.stepOffset = 0.0f;
        this.characterController.center = new Vector3(0.0f, 0.9f, 0.0f);
        this.characterController.radius = 0.3f;
        this.characterController.height = 1.8f;
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

        transform.Rotate(Vector3.up * mouseX);

        this.xRotation -= mouseY;
        this.xRotation = Mathf.Clamp(this.xRotation, -90, 90);

        this.camera.localRotation = Quaternion.Euler(this.xRotation, 0, 0);
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

    private GameObject highlight;
    //private GameObject cube;

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
                Chunk c = World.GetChunkBlock(new Vector3(
                    Mathf.FloorToInt(pointPos.x),
                    Mathf.FloorToInt(pointPos.y),
                    Mathf.FloorToInt(pointPos.z)
                ));

                c.SetBlock(pointPos, Block.AIR);
            }
            if(Input.GetMouseButtonDown(0)) {
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

                Chunk c = World.GetChunkBlock(new Vector3(
                    Mathf.FloorToInt(pointPos2.x),
                    Mathf.FloorToInt(pointPos2.y),
                    Mathf.FloorToInt(pointPos2.z)
                ));

                c.SetBlock(pointPos2, Block.STONE);
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

        //CreateCube();
    }

    /*
    private void CreateCube() {
        this.cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        this.cube.transform.parent = this.highlight.transform;
        this.cube.transform.position = new Vector3(0.5f, 0.5f, 0.5f);

        this.meshRenderer = this.cube.GetComponent<MeshRenderer>();
        this.material = Resources.Load<Material>("Materials/Block Highlight");
        this.meshRenderer.material = this.material;
    }
    */

    private void UpdateHighlight(Vector3 pointPos, Vector3 pointPos2) {
        Mesh mesh = new Mesh();
        mesh.name = "Block Highlight";

        this.vertices.Clear();
        this.triangles.Clear();

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
}
