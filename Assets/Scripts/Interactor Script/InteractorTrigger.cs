using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InteractorTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent unityEvent;
    private bool inTrigger;
    private PlayerInputActions playerInputActions;
    private InputAction interact;
    
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        interact = playerInputActions.PlayerControls.Interact;
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inTrigger = false;
        }
    }

    private void Update()
    {
        if (!inTrigger) return;
        else
        {
            if (interact.triggered)
            {
                unityEvent.Invoke();
            }
        }
    }
}
