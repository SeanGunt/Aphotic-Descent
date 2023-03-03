using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FreeFlyCam : MonoBehaviour
{
    public float cameraSensitivity = 800f;
    public float verticalSpeed = 4.0f;
    public float moveSpeed = 10.0f;
    public float slowMoveFactor = 0.25f;
    public float fastMoveFactor = 3.0f;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private float defaultYPos;
    [SerializeField]private Camera freeFlyCam;
    private PlayerInput cameraInput;
    [SerializeField]private GameObject thePlayer, playerCam, freeFlyCamera;
    [SerializeField]private PlayerMovement pC;


    private void Awake()
    {
        freeFlyCam = GetComponentInChildren<Camera>();
        defaultYPos = freeFlyCam.transform.localPosition.y;
        cameraInput = thePlayer.GetComponent<PlayerInput>();
        pC = thePlayer.GetComponent<PlayerMovement>();
    }
    
    void Start ()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    void OnEnable()
    {
        freeFlyCam.enabled = true;
        freeFlyCamera.SetActive(true);
        playerCam.SetActive(false);
        Debug.Log("I Exist.");
    }

    void OnDisable()
    {
        freeFlyCam.enabled = false;
        freeFlyCamera.SetActive(false);
        playerCam.SetActive(true);
        cameraInput.SwitchCurrentActionMap("PlayerControls");
        pC.enabled = true;
        Debug.Log("I'm gone.");
    }

    void Update ()
    {
        Vector2 mInput = cameraInput.actions["Look"].ReadValue<Vector2>();

        float mouseX = mInput.x;
        float mouseY = mInput.y;

        rotationX += mouseX * cameraSensitivity * Time.deltaTime;
        rotationY += mouseY * cameraSensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp (rotationY, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

        Vector2 input = cameraInput.actions["Movement"].ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * transform.right + move.z * transform.forward;
        
        bool isSpeedUpHeld = cameraInput.actions["SpeedUp"].ReadValue<float>() > 0.001f;
        bool isSlowDownHeld = cameraInput.actions["SlowDown"].ReadValue<float>() > 0.001f;
        
        if (isSpeedUpHeld)
        {
            transform.position += transform.forward * (moveSpeed * fastMoveFactor) * input.y * Time.deltaTime;
            transform.position += transform.right * (moveSpeed * fastMoveFactor) * input.x * Time.deltaTime;
        }
        else if (isSlowDownHeld)
        {
            transform.position += transform.forward * (moveSpeed * slowMoveFactor) * input.y * Time.deltaTime;
            transform.position += transform.right * (moveSpeed * slowMoveFactor) * input.x * Time.deltaTime;
        }
        else
        {
            transform.position += transform.forward * moveSpeed * input.y * Time.deltaTime;
            transform.position += transform.right * moveSpeed * input.x * Time.deltaTime; 
        }

        bool isUpKeyHeld = cameraInput.actions["Up"].ReadValue<float>() > 0.001f;
        
        if (isUpKeyHeld) {transform.position += transform.up * verticalSpeed * Time.deltaTime;}

        bool isDownKeyHeld = cameraInput.actions["Down"].ReadValue<float>() > 0.001f;

        if (isDownKeyHeld) {transform.position -= transform.up * verticalSpeed * Time.deltaTime;}

        if (Keyboard.current.backspaceKey.isPressed)
        {
            this.transform.position = playerCam.transform.position;
            this.transform.rotation = playerCam.transform.rotation;
            this.gameObject.SetActive(false);
        }

        if (Keyboard.current.escapeKey.isPressed)
        {
            Application.Quit();
            Debug.Log("Quit");
        }
    }
}
