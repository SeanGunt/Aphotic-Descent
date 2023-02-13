using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;
 
public class DoorKey : MonoBehaviour 
{
    public bool inTrigger;
    public GameObject tutTextObj;
    private PlayerInputActions playerInputActions;
    private GameObject Player;
    private PlayerInput playerInput;
    private InputAction interact;
    public TextMeshProUGUI tutText;
    [SerializeField] private ClearUIText clearUIText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pickupSound;

    private void Start()
    {
        if (GameDataHolder.knifeHasBeenPickedUp)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        playerInput = Player.GetComponent<PlayerInput>();
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
            if (playerInput.actions["Interact"].triggered)
            {
                audioSource.PlayOneShot(pickupSound);
                clearUIText.CancelInvoke();
                clearUIText.Invoke("ClearUI", 2);
                tutTextObj.SetActive(true);
                tutText.text = "Knife Acquired";
                GameDataHolder.knifeHasBeenPickedUp = true;
                GameDataHolder.doorKey= true;
                DataPersistenceManager.instance.SaveGame();
                Destroy(this.gameObject);
            }
        }
    }
}