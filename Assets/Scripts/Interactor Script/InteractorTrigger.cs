using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InteractorTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent unityEvent;
    private bool inTrigger;
    private PlayerInput playerInput;
    private GameObject Player;
    
    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        playerInput = Player.GetComponent<PlayerInput>();
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
            if (playerInput.actions["Interact"].triggered)
            {
                unityEvent.Invoke();
                inTrigger = false;
            }
        }
    }
}
