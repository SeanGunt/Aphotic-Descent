using UnityEngine;
using System.Collections;
using TMPro;
 
public class DoorKey : MonoBehaviour 
{
    public bool inTrigger;
    public GameObject tutTextObj;
    private PlayerInputActions playerInputActions;
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
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
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
            if (playerInputActions.PlayerControls.Interact.triggered)
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