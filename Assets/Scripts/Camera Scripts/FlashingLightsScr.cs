using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingLightsScr : MonoBehaviour
{

    public Animator animator;
    [SerializeField] private Light flashingLight;
    public float lightIntensity;

    // Start is called before the first frame update
    void Start()
    {
        flashingLight = this.GetComponent<Light>();
        animator = this.GetComponent<Animator>();
        lightIntensity = this.GetComponent<Light>().intensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.HasKey("flashingLights") && PlayerPrefs.GetInt("flashingLights") !=1)
        {
            flashingLight.intensity = 10;
            animator.speed = 0;
        }
        else
        {
            flashingLight.intensity = lightIntensity;
            animator.speed = 1;
        }
    }
}
