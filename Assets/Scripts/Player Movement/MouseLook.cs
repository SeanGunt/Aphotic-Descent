using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 30f;
   
    [SerializeField] private Transform playerBody;
    [SerializeField] private GameObject playerController;
    private float xRotation = 0f;
    private PlayerInput playerInput;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Awake()
    {
        playerInput = playerController.GetComponent<PlayerInput>();
    }

    void Update()
    {
        Vector2 mInput = playerInput.actions["Look"].ReadValue<Vector2>();
        
        float mouseX = mInput.x;
        float mouseY = mInput.y;
        
        xRotation -= (mouseY * Time.deltaTime) * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
       

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * (mouseX * Time.deltaTime) * mouseSensitivity); 
        
        if (playerInput.actions["Quit"].triggered)
        {
            Application.Quit();
            Debug.Log("Quit");
        }
       
    }
}
