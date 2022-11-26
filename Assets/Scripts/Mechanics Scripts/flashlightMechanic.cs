using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class flashlightMechanic : MonoBehaviour
{
    [SerializeField] GameObject FlashlightLight;
    [SerializeField] GameObject BlacklightLight;
    private bool flashlightOn = false;
    [SerializeField]public float flashlightBattery, maxBattery;
    [SerializeField]private Image batteryBar, fullBatteryBar, emptyBatteryBar, flashlightUI;
    [SerializeField]public Text flashlightText;
    [SerializeField]private HiddenObjectsInteraction hI;
    [SerializeField]private RevealHiddenObjects rHO;
    [SerializeField]private SpawnHiddenObject sHO;
    private BlacklightEvent blEvent;
    public bool flashlightEmpty;
    public float range = 10f;
    public Camera mainCam;
    public LayerMask layer;
    public AudioSource audioSource;
    public AudioClip flashlightOutOfWaterSound;
    public AudioClip flashlightInWaterSound;
    public GameObject player;
    private PlayerMovement PMS;

    // Start is called before the first frame update
    void Start()
    {
        PMS = player.GetComponent<PlayerMovement>();
        FlashlightLight.gameObject.SetActive(false);
        BlacklightLight.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!flashlightEmpty && GameDataHolder.flashlightHasBeenPickedUp)
        {
            if (Input.GetButtonDown("Flashlight"))
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
                    flashlightOn = true;
                }
                else
                {
                    FlashlightLight.gameObject.SetActive(false);
                    flashlightOn = false;
                    BlacklightLight.gameObject.SetActive(false);
                    Invoke("ClearUI", 4);
                }
            }
            flashlightBattery = Mathf.Clamp(flashlightBattery, 0f, maxBattery);
            if (flashlightOn)
            {
                batteryBar.fillAmount = flashlightBattery/maxBattery;
                if (Input.GetButton("Blacklight"))
                {
                    flashlightBattery -= Time.deltaTime*2;
                    BlacklightReveal();
                }
                else
                {
                    flashlightBattery -= Time.deltaTime;
                    BlacklightLight.gameObject.SetActive(false);
                    FlashlightLight.gameObject.SetActive(true);
                }
            }
            if (flashlightBattery <= 0)
            {
                flashlightOn = false;
                flashlightText.text = "You ran out of battery";
                Invoke("ClearUI", 2);
                flashlightEmpty = true;
            }
        }
        else
        {
            BlacklightLight.gameObject.SetActive(false);
            FlashlightLight.gameObject.SetActive(false);
        }  
        FlashlightUIControl();
    }
    void BlacklightReveal()
    {
        BlacklightLight.gameObject.SetActive(true);
        FlashlightLight.gameObject.SetActive(false);
        RaycastHit hit;
        if(Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, range, layer))
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
}
