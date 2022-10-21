using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class flashlightMechanic : MonoBehaviour
{
    [SerializeField] GameObject FlashlightLight;
    [SerializeField] GameObject BlacklightLight;
    private bool flashlightOn = false;
    [SerializeField]private float flashlightBattery, maxBattery;
    [SerializeField]private Image batteryBar, fullBatteryBar;
    [SerializeField]private Text interactionText;
    [SerializeField]private HiddenObjectsInteraction hI;
    [SerializeField] private RevealHiddenObjects rHO;
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
                interactionText.text = "Flashlight Off.";
                Invoke ("ClearUI", 4);
            }
        }
        flashlightBattery = Mathf.Clamp(flashlightBattery, 0f, maxBattery);
        if (flashlightOn)
        {
            interactionText.text = "Flashlight On.";
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

    }
    void BlacklightReveal()
    {
        interactionText.text = "Blacklight On.";
        BlacklightLight.gameObject.SetActive(true);
        FlashlightLight.gameObject.SetActive(false);
        RaycastHit hit;
        if(Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, range, layer))
        {
            Debug.Log("Initial Interaction check");
            if (hit.collider.GetComponent<HiddenObjectsInteraction>() != false)
            {
                hI = hit.collider.GetComponent<HiddenObjectsInteraction>();
                Debug.Log("Obj revealed");
                hI.objRevealed = true;
            }

            if (hit.collider.GetComponent<RevealHiddenObjects>() != false)
            {
                rHO = hit.collider.GetComponent<RevealHiddenObjects>();
                rHO.objRevealed = true;
            }

        }
        else
        {
            Debug.Log("No object to reveal");
        }
    }
    void ClearUI()
    {
      interactionText.text = "";
    }
}
