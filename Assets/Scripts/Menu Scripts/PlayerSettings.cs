using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    void Update()
    {
        if (PlayerPrefs.HasKey("Sensitivity") && mainCamera.activeInHierarchy)
        {
            MouseLook ml = Camera.main.GetComponent<MouseLook>();
            ml.mouseSensitivity = PlayerPrefs.GetFloat("Sensitivity")*100;
        }
    }
}
