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
    [SerializeField]private Image batteryBar, fullBatteryBar;
    [SerializeField]private Text flashlightText;
    [SerializeField]private HiddenObjectsInteraction hI;
    [SerializeField]private RevealHiddenObjects rHO;
    [SerializeField]private bool flashlightEmpty;
    public float range = 10f;
    public Camera mainCam;
    public LayerMask layer;

    // Start is called before the first frame update
    void Start()
    {
        FlashlightLight.gameObject.SetActive(false);
        BlacklightLight.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!flashlightEmpty)
        {
            if (Input.GetButtonDown("Flashlight"))
            {
                if (!flashlightOn)
                {
                    FlashlightLight.gameObject.SetActive(true);
                    flashlightOn = true;
                }
                else
                {
                    FlashlightLight.gameObject.SetActive(false);
                    flashlightOn = false;
                    BlacklightLight.gameObject.SetActive(false);
                    flashlightText.text = "Flashlight Off.";
                    Invoke ("ClearUI", 4);
                }
            }
            flashlightBattery = Mathf.Clamp(flashlightBattery, 0f, maxBattery);
            if (flashlightOn)
            {
                flashlightText.text = "Flashlight On.";
                fullBatteryBar.fillAmount = flashlightBattery/maxBattery;
                if (Input.GetButton("Blacklight"))
                {
                    flashlightBattery -= Time.deltaTime*2;
                    BlacklightReveal();
                }
                else
                {
                    flashlightBattery -= Time.deltaTime;
                    Debug.Log("Blacklight off");
                    BlacklightLight.gameObject.SetActive(false);
                    FlashlightLight.gameObject.SetActive(true);
                }
            }
            if (flashlightBattery <= 0)
            {
                flashlightBattery = 0;
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
        

    }
    void BlacklightReveal()
    {
        flashlightText.text = "Blacklight On.";
        BlacklightLight.gameObject.SetActive(true);
        FlashlightLight.gameObject.SetActive(false);
        RaycastHit hit;
        if(Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, range, layer))
        {
            Debug.Log("Initial Interaction check");
            if (hit.collider.GetComponent<HiddenObjectsInteraction>() != false)
            {
                hI = hit.collider.GetComponent<HiddenObjectsInteraction>();
                flashlightText.text = "Object Revealed";
                hI.objRevealed = true;
            }

            if (hit.collider.GetComponent<RevealHiddenObjects>() != false)
            {
                rHO = hit.collider.GetComponent<RevealHiddenObjects>();
                flashlightText.text = "Object Revealed";
                rHO.objRevealed = true;
            }
        }
        else
        {
            Debug.Log("No object to reveal");
            flashlightText.text = "Blacklight On.";
        }
    }
    void ClearUI()
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
        fullBatteryBar.fillAmount = flashlightBattery/maxBattery;
    }
}
