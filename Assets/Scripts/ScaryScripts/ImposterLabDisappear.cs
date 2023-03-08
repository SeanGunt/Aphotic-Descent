using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImposterLabDisappear : MonoBehaviour
{

    [SerializeField] private GameObject CrabLabImposter;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.Destroy(CrabLabImposter);
        }
    }
}
