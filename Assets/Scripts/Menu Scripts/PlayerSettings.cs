using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            MouseLook ml = Camera.main.GetComponent<MouseLook>();
            ml.mouseSensitivity = PlayerPrefs.GetFloat("Sensitivity")*100;
        }
    }
}
