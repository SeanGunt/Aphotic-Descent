using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 100f;
   
    [SerializeField] private Transform playerBody;
    private float xRotation = 0f;
    private PlayerInputActions playerInputActions;
    private InputAction look;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        look = playerInputActions.PlayerControls.Look;
        look.Enable();
    }

    private void OnDisable()
    {
        look.Disable();
    }

    void FixedUpdate()
    {
        Vector2 mInput = look.ReadValue<Vector2>();
        
        float mouseX = mInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mInput.y * mouseSensitivity * Time.deltaTime;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
       

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX); 
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Quit");
        }
       
    }
}
