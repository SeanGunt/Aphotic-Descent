using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 100f;
   
    [SerializeField] private Transform playerBody;
    private float xRotation = 0f;
    private PlayerInputActions playerInputActions;
    private InputAction look, escape;

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

        escape = playerInputActions.PlayerControls.Escape;
        escape.Enable();
    }

    private void OnDisable()
    {
        look.Disable();
        escape.Disable();
    }

    void Update()
    {
        //Vector2 mInput = look.ReadValue<Vector2>();
        
        /*float mouseX = mInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mInput.y * mouseSensitivity * Time.deltaTime;*/

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
       

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX); 
        
        if (escape.triggered)
        {
            Application.Quit();
            Debug.Log("Quit");
        }
       
    }
}
