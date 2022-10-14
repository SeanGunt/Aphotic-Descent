using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlightMechanic : MonoBehaviour
{
    [SerializeField] GameObject FlashlightLight;
    [SerializeField] GameObject BlacklightLight;
    private bool flashlightActive = false;
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
            if (!flashlightActive)
            {
                FlashlightLight.gameObject.SetActive(true);
                flashlightActive = true;
            }
            else
            {
                FlashlightLight.gameObject.SetActive(false);
                flashlightActive = false;
            }
        }
        if (flashlightActive)
        {
            if (Input.GetButton("Blacklight"))
            {
                BlacklightReveal();
            }
            else
            {
                Debug.Log("Blacklight off");
                BlacklightLight.gameObject.SetActive(false);
                FlashlightLight.gameObject.SetActive(true);
            }
        }

    }

    void BlacklightReveal()
    {
        BlacklightLight.gameObject.SetActive(true);
        FlashlightLight.gameObject.SetActive(false);
        RaycastHit hit;
        if(Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, range, layer))
        {
            Debug.Log("HiddenObjectRevealed");
        }
        else
        {
            Debug.Log("No object to reveal");
        }
    }
}
