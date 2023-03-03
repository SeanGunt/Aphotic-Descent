using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class flashlightMechanic : MonoBehaviour
{
    [SerializeField] GameObject FlashlightLight;
    [SerializeField] GameObject BlacklightLight;
    [HideInInspector] public static RaycastHit hit;
    private PlayerInput playerInput;
    [SerializeField]private Image blacklightBar, flashlightUI, emptyLight, onLight, blLight;
    [SerializeField]public Text flashlightText;
    private HiddenObjectsInteraction hI;
    private RevealHiddenObjects rHO;
    private SpawnHiddenObject sHO;
    private BlacklightEvent blEvent;
    public float range = 10f, blacklightChargeTime, blacklightDischargeTime;
    public Camera mainCam;
    public LayerMask layer;
    public AudioSource audioSource;
    public AudioClip flashlightOutOfWaterSound;
    public AudioClip flashlightInWaterSound;
    public AudioClip blacklightOnSound;
    private GameObject player;
    private PlayerMovement PMS;
    private State state;

    private enum State
    {
        blacklightOn, flashlightOn, flashlightOff
    }

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerInput = player.GetComponent<PlayerInput>();
        state = State.flashlightOff;
    }
    
    void Start()
    {
        PMS = player.GetComponent<PlayerMovement>();
        FlashlightLight.gameObject.SetActive(false);
        BlacklightLight.gameObject.SetActive(false);
    }

    void Update()
    {
        if (GameDataHolder.flashlightHasBeenPickedUp)
        {
            switch(state)
            {
                case State.flashlightOn:
                    HandleFlashlightOn();
                break;
                case State.blacklightOn:
                    HandleBlacklightOn();
                break;
                case State.flashlightOff:
                    HandleFlashlightOff();
                break;
            }
        }
    }

    private void HandleFlashlightOn()
    {
        if (playerInput.actions["Flashlight"].triggered)
        {
            state = State.flashlightOff;
        }

        FlashlightLight.gameObject.SetActive(true);
        BlacklightLight.gameObject.SetActive(false);
        emptyLight.gameObject.SetActive(false);
        onLight.gameObject.SetActive(true);
        blLight.gameObject.SetActive(false);

        bool isBlacklightKeyHeld = playerInput.actions["Blacklight"].ReadValue<float>() > 0.1f;
        if (isBlacklightKeyHeld)
        {
            blacklightBar.fillAmount += blacklightChargeTime * Time.deltaTime;
        }
        else
        {
            blacklightBar.fillAmount -= blacklightChargeTime * Time.deltaTime;
        }

        if(blacklightBar.fillAmount == 1)
        {
            state = State.blacklightOn;
        }
    }

    private void HandleBlacklightOn()
    {
        blLight.gameObject.SetActive(true);
        onLight.gameObject.SetActive(false);
        BlacklightReveal();
        blacklightBar.fillAmount -= blacklightDischargeTime * Time.deltaTime;
        if(blacklightBar.fillAmount == 0)
        {
            state = State.flashlightOn;
        }
    }

    private void HandleFlashlightOff()
    {
        if (GameDataHolder.flashlightHasBeenPickedUp)
        {
            emptyLight.gameObject.SetActive(true);
        }
        else
        {
            emptyLight.gameObject.SetActive(false);
        }
        blacklightBar.fillAmount -= blacklightChargeTime * Time.deltaTime;
        if (playerInput.actions["Flashlight"].triggered)
        {
            if (PMS.inWater)
            {
                audioSource.PlayOneShot(flashlightInWaterSound);
            }
            else
            {
                audioSource.PlayOneShot(flashlightOutOfWaterSound);
            }
            state = State.flashlightOn;
        }

        emptyLight.gameObject.SetActive(true);
        FlashlightLight.gameObject.SetActive(false);
        onLight.gameObject.SetActive(false);
        BlacklightLight.gameObject.SetActive(false);
    }
    void BlacklightReveal()
    {
        BlacklightLight.gameObject.SetActive(true);
        FlashlightLight.gameObject.SetActive(false);
        if(Physics.SphereCast(mainCam.transform.position, 1f, mainCam.transform.forward, out hit, range, layer) || Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, range, layer))
        { 
            if (hit.collider.GetComponent<HiddenObjectsInteraction>() != false)
            {
                hI = hit.collider.GetComponent<HiddenObjectsInteraction>();
                if (hI.objSpawned == false)
                {
                    hI.objRevealed = true;
                }
            }

            if (hit.collider.GetComponent<RevealHiddenObjects>() != false)
            {
                rHO = hit.collider.GetComponent<RevealHiddenObjects>();
                if (rHO.objSpawned == false)
                {
                    rHO.objRevealed = true;
                }
            }

            if (hit.collider.GetComponent<SpawnHiddenObject>() != false)
            {
                sHO = hit.collider.GetComponent<SpawnHiddenObject>();
                if (sHO.objSpawned == false)
                {
                    sHO.objRevealed = true;
                }
            }
            if (hit.collider.GetComponent<BlacklightEvent>() != false)
            {
                blEvent = hit.collider.GetComponent<BlacklightEvent>();
                blEvent.uEvent.Invoke();
            }
        }
    }
}