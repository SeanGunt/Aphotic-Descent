using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCamActivationScr : MonoBehaviour
{
    [SerializeField] private GameObject mirrorPlane;
    [SerializeField] private GameObject deadScientist;

    void Start()
    {
        mirrorPlane.SetActive(false);
        deadScientist.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        mirrorPlane.SetActive(true);
        deadScientist.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        mirrorPlane.SetActive(false);
        deadScientist.SetActive(false);
    }
}
