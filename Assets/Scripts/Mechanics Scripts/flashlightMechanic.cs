using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class flashlightMechanic : MonoBehaviour
{
    [SerializeField] GameObject FlashlightLight;
    [SerializeField] GameObject BlacklightLight;
    [HideInInspector] public static RaycastHit hit;
    private bool flashlightOn = false;
    private bool blacklightIsOn = false;
    private PlayerInputActions playerInputActions;
    private InputAction flashlight, blacklight;
    [SerializeField]public float flashlightBattery, maxBattery;
    [SerializeField]private Image batteryBar, fullBatteryBar, emptyBatteryBar, flashlightUI, emptyLight, onLight, blLight;
    [SerializeField]public Text flashlightText;
    [SerializeField]private HiddenObjectsInteraction hI;
    [SerializeField]private RevealHiddenObjects rHO;
    [SerializeField]private SpawnHiddenObject sHO;
    [SerializeField]private float flashlightCooldown = 15.0f;
    private float flCdResetTime;
    private BlacklightEvent blEvent;
    private bool flashlightDisable;
    public bool flashlightEmpty;
    public float range = 10f;
    public Camera mainCam;
    public LayerMask layer;
    public AudioSource audioSource;
    public AudioClip flashlightOutOfWaterSound;
    public AudioClip flashlightInWaterSound;
    public AudioClip blacklightOnSound;
    public GameObject player;
    private PlayerMovement PMS;

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        flashlight = playerInputActions.PlayerControls.Flashlight;
        blacklight = playerInputActions.PlayerControls.Blacklight;
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        PMS = player.GetComponent<PlayerMovement>();
        FlashlightLight.gameObject.SetActive(false);
        BlacklightLight.gameObject.SetActive(false);

        flCdResetTime = flashlightCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (!flashlightEmpty && GameDataHolder.flashlightHasBeenPickedUp)
        {
            //flashlight input effects start here
            if (flashlight.triggered)
            {
                if (!flashlightOn)
                {
                    if (PMS.inWater)
                    {
                        audioSource.PlayOneShot(flashlightInWaterSound);
                    }
                    else
                    {
                        audioSource.PlayOneShot(flashlightOutOfWaterSound);
                    }
                    FlashlightLight.gameObject.SetActive(true);
                    emptyLight.gameObject.SetActive(false);
                    onLight.gameObject.SetActive(true);
                    flashlightOn = true;
                }
                else
                {
                    emptyLight.gameObject.SetActive(true);
                    FlashlightLight.gameObject.SetActive(false);
                    onLight.gameObject.SetActive(false);
                    flashlightOn = false;
                    BlacklightLight.gameObject.SetActive(false);
                    blLight.gameObject.SetActive(false);
                    Invoke("ClearUI", 4);
                }
            }


            flashlightBattery = Mathf.Clamp(flashlightBattery, 0f, maxBattery);

            bool isBlacklightKeyHeld = blacklight.ReadValue<float>() > 0.1f;

            if (flashlightOn && !flashlightDisable)
            {
                batteryBar.fillAmount = flashlightBattery/maxBattery;
                if (isBlacklightKeyHeld)
                {
                    if (!blacklightIsOn)
                    {
                        blLight.gameObject.SetActive(true);
                        audioSource.PlayOneShot(blacklightOnSound);
                        blacklightIsOn = true;
                    }
                    flashlightBattery -= Time.deltaTime*2;
                    BlacklightReveal();
                }
                else
                {
                    blacklightIsOn = false;
                    blLight.gameObject.SetActive(false);
                    flashlightBattery -= Time.deltaTime;
                    BlacklightLight.gameObject.SetActive(false);
                    FlashlightLight.gameObject.SetActive(true);
                }
            }
            if (flashlightBattery <= 0)
            {
                flashlightOn = false;
                flashlightText.text = "You ran out of battery";
                emptyLight.gameObject.SetActive(true);
                onLight.gameObject.SetActive(false);
                blLight.gameObject.SetActive(false);
                Invoke("ClearUI", 2);
                flashlightEmpty = true;

                flashlightDisable = true;
            }
        }
        else
        {
            BlacklightLight.gameObject.SetActive(false);
            FlashlightLight.gameObject.SetActive(false);
        }

        if(flashlightDisable)
        {
            FlCooldown();
        }

        FlashlightUIControl();
        //flashlight input effects end here


    }
    void BlacklightReveal()
    {
        BlacklightLight.gameObject.SetActive(true);
        FlashlightLight.gameObject.SetActive(false);
        //if(Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, range, layer))
        if(Physics.SphereCast(mainCam.transform.position, 1.5f, mainCam.transform.forward, out hit, range, layer) || Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, range, layer))
        { 
            if (hit.collider.GetComponent<HiddenObjectsInteraction>() != false)
            {
                hI = hit.collider.GetComponent<HiddenObjectsInteraction>();
                if (hI.objSpawned == false)
                {
                    flashlightText.text = "Object Revealed";
                    hI.objRevealed = true;
                    Invoke("ClearUI", 3);
                }
            }

            if (hit.collider.GetComponent<RevealHiddenObjects>() != false)
            {
                rHO = hit.collider.GetComponent<RevealHiddenObjects>();
                if (rHO.objSpawned == false)
                {
                    flashlightText.text = "Object Revealed";
                    rHO.objRevealed = true;
                    Invoke("ClearUI", 3);
                }
            }

            if (hit.collider.GetComponent<SpawnHiddenObject>() != false)
            {
                sHO = hit.collider.GetComponent<SpawnHiddenObject>();
                if (sHO.objSpawned == false)
                {
                    flashlightText.text = "Object Revealed";
                    sHO.objRevealed = true;
                    Invoke("ClearUI", 3);
                }
            }
            if (hit.collider.GetComponent<BlacklightEvent>() != false)
            {
                blEvent = hit.collider.GetComponent<BlacklightEvent>();
                blEvent.uEvent.Invoke();
            }
        }
    }
    public void ClearUI()
    {
      flashlightText.text = "";
    }


    public void FillBattery(float amount)
    {
        flashlightBattery += amount;
        if (flashlightBattery >= maxBattery)
        {
            flashlightBattery = maxBattery;
        }
        batteryBar.fillAmount = flashlightBattery/maxBattery;
    }


    void FlashlightUIControl()
    {
        if (GameDataHolder.flashlightHasBeenPickedUp == true)
        {
            flashlightUI.gameObject.SetActive(true);
            if (flashlightBattery >= maxBattery && !flashlightEmpty)
            {
                fullBatteryBar.gameObject.SetActive(true);
                batteryBar.gameObject.SetActive(false);
                emptyBatteryBar.gameObject.SetActive(false);
            }
            else if (flashlightBattery < maxBattery && !flashlightEmpty)
            {
                fullBatteryBar.gameObject.SetActive(false);
                batteryBar.gameObject.SetActive(true);
                emptyBatteryBar.gameObject.SetActive(true);
            }
            else
            {
                fullBatteryBar.gameObject.SetActive(false);
                batteryBar.gameObject.SetActive(false);
                emptyBatteryBar.gameObject.SetActive(true);
            }
        }
        else
        {
            flashlightUI.gameObject.SetActive(false);
        }
    }

    void FlCooldown()
    {  
        flashlightCooldown -= Time.deltaTime;
        if(flashlightCooldown <= 0)
        {
            flashlightDisable = false;
            flashlightCooldown = flCdResetTime;

            FillBattery(maxBattery);
            flashlightEmpty = false;

            flashlightText.text = "Flashlight Battery Recharged";
            Invoke("ClearUI", 3);
        }   
    }
}