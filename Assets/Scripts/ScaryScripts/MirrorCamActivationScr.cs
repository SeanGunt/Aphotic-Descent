using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCamActivationScr : MonoBehaviour
{
    [SerializeField] private GameObject mirrorPlane;

    void Start()
    {
        mirrorPlane.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        mirrorPlane.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        mirrorPlane.SetActive(false);
    }
}
