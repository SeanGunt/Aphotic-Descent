using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 30f;
   
    [SerializeField] private Transform playerBody;
    [SerializeField] private GameObject playerController;
    private float xRotation = 0f;
    //private PlayerInputActions playerInputActions;
    private PlayerInput playerInput;
    //private InputAction look, escape;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Awake()
    {
        //playerInputActions = new PlayerInputActions();
        playerInput = playerController.GetComponent<PlayerInput>();
    }

    // private void OnEnable()
    // {
    //     look = playerInputActions.PlayerControls.Look;
    //     look.Enable();

    //     escape = playerInputActions.PlayerControls.Escape;
    //     escape.Enable();
    // }

    // private void OnDisable()
    // {
    //     look.Disable();
    //     escape.Disable();
    // }

    void Update()
    {
        Vector2 mInput = playerInput.actions["Look"].ReadValue<Vector2>();
        
        float mouseX = mInput.x;
        float mouseY = mInput.y;
        
        xRotation -= (mouseY * Time.deltaTime) * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
       

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * (mouseX * Time.deltaTime) * mouseSensitivity); 
        
        if (playerInput.actions["Escape"].triggered)
        {
            Application.Quit();
            Debug.Log("Quit");
        }
       
    }
}
