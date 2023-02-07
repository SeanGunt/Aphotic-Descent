using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class FlashlightPickup : MonoBehaviour
{
    public bool inTrigger;
    public GameObject tutTextObj;
    public TextMeshProUGUI tutText;
    [SerializeField] private ClearUIText clearUIText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pickupSound;
    private PlayerInputActions playerInputActions;
    private InputAction interact;

    private void Start()
    {
        if (GameDataHolder.flashlightHasBeenPickedUp)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        interact = playerInputActions.PlayerControls.Interact;
        interact.Enable();
    }

    private void OnDisable()
    {
        interact.Disable();
    }
 
    void OnTriggerEnter(Collider other)
    {
        inTrigger = true;
    }
 
    void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }
 
    void Update()
    {
        if (inTrigger)
        {
            if (interact.triggered)
            {
                audioSource.PlayOneShot(pickupSound);
                clearUIText.CancelInvoke();
                clearUIText.Invoke("ClearUI", 2);
                tutTextObj.SetActive(true);
                tutText.text = "Flashlight Acquired";
                GameDataHolder.flashlightHasBeenPickedUp = true;
                DataPersistenceManager.instance.SaveGame();
                Destroy(this.gameObject);
            }
        }
    }
}
